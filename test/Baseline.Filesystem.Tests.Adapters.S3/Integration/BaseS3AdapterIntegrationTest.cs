using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using FluentAssertions;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration
{
    public abstract class BaseS3AdapterIntegrationTest : IAsyncDisposable
    {
        private static readonly Random Random = new Random();

        protected string RootPath { get; private set; }
        protected readonly string GeneratedBucketName;
        protected readonly IAmazonS3 S3Client;
        
        protected IFileManager FileManager;
        protected IDirectoryManager DirectoryManager;

        protected BaseS3AdapterIntegrationTest()
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
            
            ReconfigureManagerInstances(false);
        }

        public async ValueTask DisposeAsync()
        {
            await S3Client.DeleteBucketAsync(GeneratedBucketName);
        }

        protected void ReconfigureManagerInstances(bool useRootPath)
        {
            RootPath = useRootPath ? $"{RandomString(6)}/{RandomString(2)}" : null;
            
            var adapter = new S3Adapter(new S3AdapterConfiguration
            {
                BucketName = GeneratedBucketName,
                S3Client = S3Client
            });
            
            var adapterManager = new AdapterManager();
            adapterManager.Register(new AdapterRegistration
            {
                Adapter = adapter, 
                RootPath = RootPath?.AsBaselineFilesystemPath()
            });
            
            FileManager = new FileManager(adapterManager);
            DirectoryManager = new DirectoryManager(adapterManager);
        }

        protected async Task CreateFileAndWriteTextAsync(PathRepresentation path, string contents = "")
        {
            if (await FileExistsAsync(path))
            {
                throw new Exception("File already exists!");
            }

            await S3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = GeneratedBucketName,
                Key = CombinePathWithRootPath(path),
                ContentBody = contents
            });
        }

        protected async Task ExpectFileToExistAsync(PathRepresentation path)
        {
            var exists = await FileExistsAsync(path);
            exists.Should().BeTrue();
        }

        protected async Task ExpectFileNotToExistAsync(PathRepresentation path)
        {
            var exists = await FileExistsAsync(path);
            exists.Should().BeFalse();
        }

        protected async Task ExpectDirectoryToExistAsync(PathRepresentation path)
        {
            var exists = await DirectoryExistsAsync(path);
            exists.Should().BeTrue();
        }

        protected async Task ExpectDirectoryNotToExistAsync(PathRepresentation path)
        {
            var exists = await DirectoryExistsAsync(path);
            exists.Should().BeFalse();
        }

        protected async Task<string> ReadFileAsStringAsync(PathRepresentation path)
        {
            var file = await S3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = GeneratedBucketName,
                Key = CombinePathWithRootPath(path)
            });

            return await new StreamReader(file.ResponseStream).ReadToEndAsync();
        }

        protected string CombinePathWithRootPath(PathRepresentation path)
        {
            return $"{(RootPath != null ? $"{RootPath}/" : string.Empty )}{path.NormalisedPath}";
        }

        protected PathRepresentation CombinedPathWithRootPathForAssertion(PathRepresentation path)
        {
            return RootPath == null ? path : new PathCombinationBuilder(RootPath.AsBaselineFilesystemPath(), path).Build();
        }

        protected static string RandomDirectoryPath(bool includeBlank = false)
        {
            var directories = new[]
            {
                includeBlank ? "" : RandomString(), 
                $"{RandomString(12)}/{RandomString(4)}",
                $"{RandomString(4)}/{RandomString(6)}/{RandomString(3)}/{RandomString(8)}", 
                $"{RandomString(6)}/{RandomString(3)}",
                RandomString(12), 
                RandomString(18)
            };
            var randomDirectory = directories[Random.Next(directories.Length)];

            return string.IsNullOrWhiteSpace(randomDirectory) ? randomDirectory : $"{randomDirectory}/";
        }

        protected static PathRepresentation RandomDirectoryPathRepresentation()
        {
            return RandomDirectoryPath().AsBaselineFilesystemPath();
        }

        protected static PathRepresentation RandomFilePathRepresentation()
        {
            var extensions = new[] {"txt", "jpg", "pdf", ".config.json" };
            var fileNames = new[]
            {
                $".{RandomString()}", 
                $".{RandomString()}.config", 
                $"{RandomString()}.{extensions[Random.Next(extensions.Length)]}"
            };

            return $"{RandomDirectoryPath(true)}{fileNames[Random.Next(fileNames.Length)]}"
                .AsBaselineFilesystemPath();
        }

        protected static PathRepresentation RandomFilePathRepresentationWithPrefix(string prefix)
        {
            return $"{prefix}/{RandomFilePathRepresentation().OriginalPath}".AsBaselineFilesystemPath();
        }

        private async Task<bool> FileExistsAsync(PathRepresentation path)
        {
            try
            {
                var file = await S3Client.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = GeneratedBucketName,
                    Key = CombinePathWithRootPath(path)
                });
                return file.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (AmazonS3Exception e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                throw;
            }
        }

        private async Task<bool> DirectoryExistsAsync(PathRepresentation path)
        {
            var objects = await S3Client.ListObjectsAsync(new ListObjectsRequest
            {
                BucketName = GeneratedBucketName,
                Prefix = CombinePathWithRootPath(path)
            });
            return objects != null && objects.S3Objects.Any();
        }
        
        private static string RandomString(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
