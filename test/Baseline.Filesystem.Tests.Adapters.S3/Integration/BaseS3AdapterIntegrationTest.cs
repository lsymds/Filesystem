using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Baseline.Filesystem.Adapters.S3;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration
{
    public abstract class BaseS3AdapterIntegrationTest : IAsyncDisposable
    {
        private static readonly Random Random = new Random();

        private readonly bool _useRootPath;
        
        protected readonly string GeneratedBucketName;
        protected readonly IAmazonS3 S3Client;
        protected readonly IFileManager FileManager;
        protected readonly IDirectoryManager DirectoryManager;

        protected BaseS3AdapterIntegrationTest(bool useRootPath = false)
        {
            _useRootPath = useRootPath;
            
            GeneratedBucketName = Guid.NewGuid().ToString();
            
            S3Client = new AmazonS3Client(
                new BasicAWSCredentials("abc", "def"),
                new AmazonS3Config
                {
                    AuthenticationRegion = "eu-west-1",
                    ServiceURL = "http://localhost:4566",
                    ForcePathStyle = true,
                }
            );

            S3Client.PutBucketAsync(GeneratedBucketName).Wait();

            var adapter = new S3Adapter(new S3AdapterConfiguration
            {
                BucketName = GeneratedBucketName,
                S3Client = S3Client,
                RootPath = useRootPath ? "root/path" : null
            });
            
            var adapterManager = new AdapterManager();
            adapterManager.Register(adapter);
            
            FileManager = new FileManager(adapterManager);
            DirectoryManager = new DirectoryManager(adapterManager);
        }

        public async ValueTask DisposeAsync()
        {
            await S3Client.DeleteBucketAsync(GeneratedBucketName);
        }

        protected Task CreateFileAndWriteTextAsync(PathRepresentation path, string contents = "")
        {
            return FileManager.WriteTextAsync(new WriteTextToFileRequest {FilePath = path, TextToWrite = contents });
        }
        
        protected Task<bool> FileExistsAsync(PathRepresentation path)
        {
            return FileManager.ExistsAsync(new FileExistsRequest {FilePath = path});
        }

        protected Task<string> ReadFileAsStringAsync(PathRepresentation path)
        {
            return FileManager.ReadAsStringAsync(new ReadFileAsStringRequest {FilePath = path});
        }

        protected async Task<bool> DirectoryExistsAsync(PathRepresentation path)
        {
            var objects = await S3Client.ListObjectsAsync(new ListObjectsRequest
            {
                BucketName = GeneratedBucketName,
                Prefix = $"{(_useRootPath ? "root/path" : string.Empty)}/{path.NormalisedPath}/"
            });
            return objects != null && objects.S3Objects.Any();
        }

        protected static string RandomDirectoryPath(bool includeBlank = false)
        {
            var directories = new[] {includeBlank ? "" : "c", "a/b", "a/b/c/d", "d/e/f/g/h", "longer", "longer-still"};
            var randomDirectory = directories[Random.Next(directories.Length)];

            return string.IsNullOrWhiteSpace(randomDirectory) ? randomDirectory : $"{randomDirectory}/";
        }

        protected static PathRepresentation RandomDirectoryPathRepresentation()
        {
            return RandomDirectoryPath().AsBaselineFilesystemPath();
        }

        protected static PathRepresentation RandomFilePathRepresentation()
        {
            var extensions = new[] {"txt", "", "jpg", "pdf", ".config.json" };
            var fileNames = new[]
            {
                ".npmrc", 
                ".npmrc.config", 
                $"{Guid.NewGuid().ToString()}.{extensions[Random.Next(extensions.Length)]}"
            };

            return $"{RandomDirectoryPath(true)}{fileNames[Random.Next(fileNames.Length)]}"
                .AsBaselineFilesystemPath();
        }

        protected static PathRepresentation RandomFilePathRepresentationWithPrefix(string prefix)
        {
            return $"{prefix}/{RandomFilePathRepresentation().OriginalPath}".AsBaselineFilesystemPath();
        }
    }
}
