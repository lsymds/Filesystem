using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Directories
{
    public class CopyDirectoryTests : BaseS3AdapterIntegrationTest
    {
        private readonly PathRepresentation _sourceDirectory = RandomDirectoryPathRepresentation();
        private readonly PathRepresentation _destinationDirectory = RandomDirectoryPathRepresentation();
        
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Source_Directory_Does_Not_Exist()
        {
            Func<Task> func = async () => await DirectoryManager.CopyAsync(new CopyDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            });
            await func.Should().ThrowExactlyAsync<DirectoryNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Destination_Directory_Exists()
        {
            await CreateFileAndWriteTextAsync(
                $"{_sourceDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath()
            );
            await CreateFileAndWriteTextAsync(
                $"{_destinationDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath()
            );
            
            Func<Task> func = async () => await DirectoryManager.CopyAsync(new CopyDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            });
            await func.Should().ThrowExactlyAsync<DirectoryAlreadyExistsException>();
        }

        [Fact]
        public async Task It_Copies_A_Simple_Directory_Structure_From_One_Location_To_Another()
        {
            var files = new[]
            {
                RandomFilePathRepresentationWithPrefix(_sourceDirectory.NormalisedPath),
                RandomFilePathRepresentationWithPrefix(_sourceDirectory.NormalisedPath),
                RandomFilePathRepresentationWithPrefix(_sourceDirectory.NormalisedPath),
                RandomFilePathRepresentationWithPrefix(_sourceDirectory.NormalisedPath),
                RandomFilePathRepresentationWithPrefix(_sourceDirectory.NormalisedPath),
            };
            foreach (var file in files)
            {
                await CreateFileAndWriteTextAsync(file);
            }

            await DirectoryManager.CopyAsync(
                new CopyDirectoryRequest
                {
                    SourceDirectoryPath = _sourceDirectory,
                    DestinationDirectoryPath = _destinationDirectory
                }
            );

            foreach (var file in files)
            {
                var newDirectoryPath = file.NormalisedPath
                    .Replace(_sourceDirectory.NormalisedPath, _destinationDirectory.NormalisedPath);

                (await FileExistsAsync(newDirectoryPath.AsBaselineFilesystemPath())).Should().BeTrue();
            }
        }

        [Fact]
        public async Task It_Copies_A_Directory_Structure_With_A_Repeated_Directory_Name_Correctly()
        {
            var originalDirectory = "cheese/".AsBaselineFilesystemPath();
            var originalFile = "cheese/cheese/more-cheese/my-favourite-cheesestring.jpeg".AsBaselineFilesystemPath();
            
            await CreateFileAndWriteTextAsync(originalFile);

            await DirectoryManager.CopyAsync(new CopyDirectoryRequest
            {
                SourceDirectoryPath = originalDirectory,
                DestinationDirectoryPath = "food/".AsBaselineFilesystemPath()
            });

            var fileExistsInNewDestination = await FileExistsAsync(
                "food/cheese/more-cheese/my-favourite-cheesestring.jpeg".AsBaselineFilesystemPath()
            );
            fileExistsInNewDestination.Should().BeTrue();
        }

        [Fact]
        public async Task It_Copies_A_More_Complex_Directory_Structure_From_One_Location_To_Another()
        {
            
        }

        [Fact]
        public async Task It_Copies_A_Large_Directory_Structure_From_One_Location_To_Another()
        {
            
        }

        [Fact]
        public async Task It_Successfully_Copies_A_Directory_With_A_Root_Path()
        {
            
        }
    }
}
