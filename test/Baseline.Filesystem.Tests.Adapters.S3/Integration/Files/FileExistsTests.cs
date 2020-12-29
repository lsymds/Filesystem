using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class FileExistsTests : BaseS3AdapterIntegrationTest
    {
        [Fact]
        public async Task It_Returns_True_When_A_File_Does_Exist()
        {
            var path = "foo/bar/a-file-that-exists.txt".AsBaselineFilesystemPath();

            await FileManager.TouchAsync(
                new TouchFileRequest
                {
                    FilePath = path
                }
            );

            var response = await FileManager.ExistsAsync(
                new FileExistsRequest
                {
                    FilePath = path
                }
            );
            response.Should().BeTrue();
        }

        [Fact]
        public async Task It_Returns_False_When_A_File_Does_Not_Exist()
        {
            var response = await FileManager.ExistsAsync(
                new FileExistsRequest
                {
                    FilePath = "i-must-not-tell-lies-i-must-not-tell-lies.txt".AsBaselineFilesystemPath()
                }
            );
            response.Should().BeFalse();
        }
    }
}
