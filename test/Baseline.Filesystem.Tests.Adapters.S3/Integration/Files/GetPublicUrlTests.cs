using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class GetPublicUrlTests : BaseS3AdapterIntegrationTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_The_File_Does_Not_Exist()
        {
            Func<Task> act = async () => await FileManager.GetPublicUrlAsync(new GetFilePublicUrlRequest
            {
                FilePath = RandomFilePathRepresentation()
            });
            await act.Should().ThrowExactlyAsync<FileNotFoundException>();
        }

        [Fact]
        public async Task It_Returns_The_Public_Url_If_The_File_Exists()
        {
            var path = RandomFilePathRepresentationWithPrefix("abc");

            await CreateFileAndWriteTextAsync(path);

            var response = await FileManager.GetPublicUrlAsync(new GetFilePublicUrlRequest
            {
                FilePath = path,
                Expiry = DateTime.Today.AddDays(1)
            });
            response.Expiry.Should().Be(DateTime.Today.AddDays(1));
            response.Url.Should().StartWith($"https://localhost:4566/{GeneratedBucketName}/{path.OriginalPath}");
            response.Url.Should().Contain("X-Amz-Expires");
            response.Url.Should().Contain("X-Amz-Algorithm");
            response.Url.Should().Contain("X-Amz-SignedHeaders");
        }

        [Fact]
        public async Task It_Defaults_The_Expiry_Date()
        {
            var path = RandomFilePathRepresentationWithPrefix("abc");

            await CreateFileAndWriteTextAsync(path);

            var response = await FileManager.GetPublicUrlAsync(new GetFilePublicUrlRequest
            {
                FilePath = path
            });
            response.Expiry.Should().Be(DateTime.Today.AddDays(1));
        }

        [Fact]
        public async Task It_Retrieves_A_Public_Url_For_A_File_Under_A_Root_Path()
        {
            ReconfigureManagerInstances(true);
            
            var path = RandomFilePathRepresentationWithPrefix("abc");

            await CreateFileAndWriteTextAsync(path);

            var response = await FileManager.GetPublicUrlAsync(new GetFilePublicUrlRequest
            {
                FilePath = path,
                Expiry = DateTime.Today.AddDays(1)
            });
            response.Expiry.Should().Be(DateTime.Today.AddDays(1));
            response.Url.Should().StartWith($"https://localhost:4566/{GeneratedBucketName}/{CombinePathWithRootPath(path)}");
            response.Url.Should().Contain("X-Amz-Expires");
            response.Url.Should().Contain("X-Amz-Algorithm");
            response.Url.Should().Contain("X-Amz-SignedHeaders");
        }
    }
}
