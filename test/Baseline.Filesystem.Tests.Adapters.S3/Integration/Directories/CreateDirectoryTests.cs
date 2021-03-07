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
            var directory = RandomDirectoryPathRepresentation();
            var pathWithDirectory = RandomFilePathRepresentationWithPrefix(directory.OriginalPath);
            
            await CreateFileAndWriteTextAsync(pathWithDirectory);

            Func<Task> func = async () => await DirectoryManager.CreateAsync(new CreateDirectoryRequest
            {
                DirectoryPath = directory
            });
            await func.Should().ThrowAsync<DirectoryAlreadyExistsException>();
        }

        [Fact]
        public async Task It_Creates_The_Directory()
        {
            var directory = RandomDirectoryPathRepresentation();

            var response = await DirectoryManager.CreateAsync(new CreateDirectoryRequest {DirectoryPath = directory});
            
            await ExpectDirectoryToExistAsync(directory);
            response.Path.Should().BeEquivalentTo(directory);
        }

        [Fact]
        public async Task It_Creates_The_Directory_Under_A_Root_Path()
        {
            ReconfigureManagerInstances(true);
            
            var directory = RandomDirectoryPathRepresentation();

            var response = await DirectoryManager.CreateAsync(new CreateDirectoryRequest {DirectoryPath = directory});

            await ExpectDirectoryToExistAsync(directory);
            response.Path.Should().BeEquivalentTo(CombinedPathWithRootPathForAssertion(directory));
        }
    }
}
