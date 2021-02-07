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
                new GetFileRequest {FilePath = RandomFilePathRepresentation()}
            );
            response.File.Should().BeNull();
        }

        [Fact]
        public async Task It_Returns_The_File_If_It_Does_Exist()
        {
            var path = RandomFilePathRepresentation();

            await CreateFileAndWriteTextAsync(path);

            var response = await FileManager.GetAsync(new GetFileRequest {FilePath = path});
            response.File.Path.Should().Be(path);
        }

        [Fact]
        public async Task It_Retrieves_A_File_Under_A_Root_Path()
        {
            ReconfigureManagerInstances(true);
            
            var path = RandomFilePathRepresentation();

            await CreateFileAndWriteTextAsync(path);

            var response = await FileManager.GetAsync(new GetFileRequest {FilePath = path});
            response.File.Path.Should().BeEquivalentTo(CombinedPathWithRootPathForAssertion(path));
        }
    }
}
