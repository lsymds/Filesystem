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
            // Act.
            var response = await FileManager.GetAsync(
                new GetFileRequest {FilePath = RandomFilePathRepresentation()}
            );
            
            // Assert.
            response.Should().BeNull();
        }

        [Fact]
        public async Task It_Returns_The_File_If_It_Does_Exist()
        {
            // Arrange.
            var path = RandomFilePathRepresentation();

            // Act.
            await CreateFileAndWriteTextAsync(path);
            
            // Assert.
            var response = await FileManager.GetAsync(new GetFileRequest {FilePath = path});
            response.File.Path.Should().Be(path);
        }

        [Fact]
        public async Task It_Retrieves_A_File_Under_A_Root_Path()
        {
            // Arrange.
            ReconfigureManagerInstances(true);
            
            var path = RandomFilePathRepresentation();
            
            await CreateFileAndWriteTextAsync(path);

            // Act.
            var response = await FileManager.GetAsync(new GetFileRequest {FilePath = path});
            
            // Assert.
            response
                .File
                .Path
                .Should()
                .BeEquivalentTo(path, x => x.Excluding(y => y.GetPathTree));
        }
    }
}
