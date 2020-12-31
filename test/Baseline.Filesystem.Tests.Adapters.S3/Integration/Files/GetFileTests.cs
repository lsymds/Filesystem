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
            var path = "this-file-does-really-not-exist-001.jpg.png".AsBaselineFilesystemPath();
            
            var response = await FileManager.GetAsync(new GetFileRequest {FilePath = path});
            response.File.Should().BeNull();
        }

        [Fact]
        public async Task It_Returns_The_File_If_It_Does_Exist()
        {
            var path = "please-exist.txt".AsBaselineFilesystemPath();

            await FileManager.WriteTextAsync(new WriteTextToFileRequest
            {
                FilePath = path,
                TextToWrite = "pls"
            });

            var response = await FileManager.GetAsync(new GetFileRequest {FilePath = path});
            response.File.Path.Should().Be(path);
        }
    }
}
