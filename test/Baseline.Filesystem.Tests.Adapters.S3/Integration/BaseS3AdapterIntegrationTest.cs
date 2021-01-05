using System;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Baseline.Filesystem.Adapters.S3;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration
{
    public abstract class BaseS3AdapterIntegrationTest : IAsyncDisposable
    {
        private static readonly Random Random = new Random();
        
        protected readonly string GeneratedBucketName;
        protected readonly IAmazonS3 S3Client;
        protected readonly IFileManager FileManager;
        protected readonly IDirectoryManager DirectoryManager;

        protected BaseS3AdapterIntegrationTest(bool useRootPath = false)
        {
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

        protected static PathRepresentation RandomFilePath()
        {
            var directories = new[] {"", "a/b", "a/b/c/d", "d/e/f/g/h", "longer", "longer-still"};
            var extensions = new[] {"txt", "", "jpg", "pdf", ".config.json" };
            var fileNames = new[]
            {
                ".npmrc", 
                ".npmrc.config", 
                $"{Guid.NewGuid().ToString()}.{extensions[Random.Next(extensions.Length)]}"
            };

            var combinedPath = $"{directories[Random.Next(directories.Length)]}/{fileNames[Random.Next(fileNames.Length)]}";
            return combinedPath.AsBaselineFilesystemPath();
        }

        protected static PathRepresentation RandomFilePathWithPrefix(string prefix)
        {
            return $"{prefix}/{RandomFilePath().NormalisedPath}".AsBaselineFilesystemPath();
        }
    }
}
