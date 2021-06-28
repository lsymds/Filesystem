using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Directories
{
    public class CreateDirectoryTests : BaseS3AdapterIntegrationTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Directory_Already_Exists()
        {
            // Arrange.
            var directory = RandomDirectoryPathRepresentation();
            var pathWithDirectory = RandomFilePathRepresentationWithPrefix(directory.OriginalPath);
            
            await CreateFileAndWriteTextAsync(pathWithDirectory);

            // Act.
            Func<Task> func = async () => await DirectoryManager.CreateAsync(new CreateDirectoryRequest
            {
                DirectoryPath = directory
            });
            
            // Assert.
            await func.Should().ThrowAsync<DirectoryAlreadyExistsException>();
        }

        [Fact]
        public async Task It_Creates_The_Directory()
        {
            // Arrange.
            var directory = RandomDirectoryPathRepresentation();

            // Act.
            var response = await DirectoryManager.CreateAsync(new CreateDirectoryRequest {DirectoryPath = directory});
            
            // Assert.
            await ExpectDirectoryToExistAsync(directory);
            response.Directory.Path.Should().BeEquivalentTo(directory);
        }

        [Fact]
        public async Task It_Creates_The_Directory_Under_A_Root_Path()
        {
            // Arrange.
            ReconfigureManagerInstances(true);
            
            var directory = RandomDirectoryPathRepresentation();
            
            // Act.
            var response = await DirectoryManager.CreateAsync(new CreateDirectoryRequest {DirectoryPath = directory});
            
            // Assert.
            await ExpectDirectoryToExistAsync(directory);
            response
                .Directory
                .Path
                .Should()
                .BeEquivalentTo(directory, x => x.Excluding(y => y.GetPathTree));
        }
    }
}
