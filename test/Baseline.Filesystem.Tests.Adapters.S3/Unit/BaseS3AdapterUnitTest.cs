using Amazon.S3;
using Moq;
using Baseline.Filesystem.Adapters.S3;

namespace Baseline.Filesystem.Tests.Adapters.S3.Unit
{
    public abstract class BaseS3AdapterUnitTest
    {
        protected readonly Mock<IAmazonS3> S3Client;
        protected readonly S3Adapter S3Adapter;

        protected BaseS3AdapterUnitTest()
        {
            S3Client = new Mock<IAmazonS3>();
            S3Adapter = new S3Adapter(new S3AdapterConfiguration
            {
                BucketName = "unit-test-bucket",
                S3Client = S3Client.Object
            });
        }
    }
}
