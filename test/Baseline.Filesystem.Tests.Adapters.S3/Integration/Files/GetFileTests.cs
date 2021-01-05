using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class GetFileTests : BaseS3AdapterIntegrationTest
    {
        [Fact]
        public async Task It_Returns_Null_If_File_Does_Not_Exist()
        {
            var response = await FileManager.GetAsync(
                new GetFileRequest {FilePath = RandomFilePath().AsBaselineFilesystemPath()}
            );
            response.File.Should().BeNull();
        }

        [Fact]
        public async Task It_Returns_The_File_If_It_Does_Exist()
        {
            var path = RandomFilePath().AsBaselineFilesystemPath();

            await CreateFileAndWriteTextAsync(path);

            var response = await FileManager.GetAsync(new GetFileRequest {FilePath = path});
            response.File.Path.Should().Be(path);
        }
    }
}
