using System;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Baseline.Filesystem.Adapters.S3;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration
{
    public abstract class BaseS3AdapterIntegrationTest : IAsyncDisposable
    {
        protected readonly string GeneratedBucketName;
        protected readonly IAmazonS3 S3Client;
        protected readonly IFileManager FileManager;

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
        }

        public async ValueTask DisposeAsync()
        {
            await S3Client.DeleteBucketAsync(GeneratedBucketName);
        }
        
        protected Task<bool> FileExistsAsync(PathRepresentation path)
        {
            return FileManager.ExistsAsync(new FileExistsRequest {FilePath = path});
        }

        protected Task<string> ReadFileAsStringAsync(PathRepresentation path)
        {
            return FileManager.ReadAsStringAsync(new ReadFileAsStringRequest {FilePath = path});
        }
    }
}
