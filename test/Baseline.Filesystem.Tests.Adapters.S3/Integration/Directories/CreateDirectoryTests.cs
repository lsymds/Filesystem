using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Directories
{
    public class CreateDirectoryTests : BaseS3AdapterIntegrationTest
    {
        public CreateDirectoryTests() : base(true)
        {
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Directory_Already_Exists()
        {
            var directory = RandomDirectoryPath();
            var pathWithDirectory = RandomFilePathWithPrefix(directory.OriginalPath);
            
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
            var directory = RandomDirectoryPath();

            var response = await DirectoryManager.CreateAsync(new CreateDirectoryRequest {DirectoryPath = directory});
            
            (await DirectoryExistsAsync(directory)).Should().BeTrue();
            response.Directory.Path.Should().BeEquivalentTo(directory);
        }
    }
}
