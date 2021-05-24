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
            // Arrange.
            var path = RandomFilePathRepresentation();

            await CreateFileAndWriteTextAsync(path);

            // Act.
            var response = await FileManager.ExistsAsync(
                new FileExistsRequest
                {
                    FilePath = path
                }
            );
            
            // Assert.
            response.FileExists.Should().BeTrue();
        }

        [Fact]
        public async Task It_Returns_False_When_A_File_Does_Not_Exist()
        {
            // Act.
            var response = await FileManager.ExistsAsync(
                new FileExistsRequest
                {
                    FilePath = RandomFilePathRepresentation()
                }
            );
            
            // Assert.
            response.FileExists.Should().BeFalse();
        }

        [Fact]
        public async Task It_Returns_True_When_A_File_Exists_Under_A_Root_Path()
        {
            // Arrange.
            ReconfigureManagerInstances(true);
            
            var path = RandomFilePathRepresentation();

            await CreateFileAndWriteTextAsync(path);

            // Act.
            var response = await FileManager.ExistsAsync(
                new FileExistsRequest
                {
                    FilePath = path
                }
            );
            
            // Assert.
            response.FileExists.Should().BeTrue();
        }
    }
}
