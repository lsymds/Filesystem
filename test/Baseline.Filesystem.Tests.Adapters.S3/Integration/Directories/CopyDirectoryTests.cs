using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Directories
{
    public class CopyDirectoryTests : BaseS3AdapterIntegrationTest
    {
        private readonly PathRepresentation _sourceDirectory = RandomDirectoryPathRepresentation();
        private readonly PathRepresentation _destinationDirectory =
            RandomDirectoryPathRepresentation();

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Source_Directory_Does_Not_Exist()
        {
            // Act.
            Func<Task> func = async () =>
                await DirectoryManager.CopyAsync(
                    new CopyDirectoryRequest
                    {
                        SourceDirectoryPath = _sourceDirectory,
                        DestinationDirectoryPath = _destinationDirectory
                    }
                );

            // Assert.
            await func.Should().ThrowExactlyAsync<DirectoryNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Destination_Directory_Exists()
        {
            // Arrange.
            await CreateFileAndWriteTextAsync(
                $"{_sourceDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath()
            );
            await CreateFileAndWriteTextAsync(
                $"{_destinationDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath()
            );

            // Act.
            Func<Task> func = async () =>
                await DirectoryManager.CopyAsync(
                    new CopyDirectoryRequest
                    {
                        SourceDirectoryPath = _sourceDirectory,
                        DestinationDirectoryPath = _destinationDirectory
                    }
                );

            // Assert.
            await func.Should().ThrowExactlyAsync<DirectoryAlreadyExistsException>();
        }

        [Fact]
        public async Task It_Copies_A_Simple_Directory_Structure_From_One_Location_To_Another()
        {
            // Arrange.
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

            // Act.
            await DirectoryManager.CopyAsync(
                new CopyDirectoryRequest
                {
                    SourceDirectoryPath = _sourceDirectory,
                    DestinationDirectoryPath = _destinationDirectory
                }
            );

            // Assert.
            foreach (var file in files)
            {
                var newDirectoryPath = file.NormalisedPath.Replace(
                    _sourceDirectory.NormalisedPath,
                    _destinationDirectory.NormalisedPath
                );

                await ExpectFileToExistAsync(newDirectoryPath.AsBaselineFilesystemPath());
            }
        }

        [Fact]
        public async Task It_Copies_A_Directory_Structure_With_A_Repeated_Directory_Name_Correctly()
        {
            // Arrange.
            var originalDirectory = "cheese/".AsBaselineFilesystemPath();
            var originalFile =
                "cheese/cheese/more-cheese/my-favourite-cheesestring.jpeg".AsBaselineFilesystemPath();

            await CreateFileAndWriteTextAsync(originalFile);

            // Act.
            await DirectoryManager.CopyAsync(
                new CopyDirectoryRequest
                {
                    SourceDirectoryPath = originalDirectory,
                    DestinationDirectoryPath = "food/".AsBaselineFilesystemPath()
                }
            );

            // Assert.
            await ExpectFileToExistAsync(
                "food/cheese/more-cheese/my-favourite-cheesestring.jpeg".AsBaselineFilesystemPath()
            );
        }

        [Fact]
        public async Task It_Copies_A_More_Complex_Directory_Structure_From_One_Location_To_Another()
        {
            // Arrange.
            await CreateFileAndWriteTextAsync(
                "a/more/complex/directory/structure/.keep".AsBaselineFilesystemPath()
            );
            await CreateFileAndWriteTextAsync(
                "a/more/complex/directory/structure/with/a/nested/file.jpeg".AsBaselineFilesystemPath()
            );
            await CreateFileAndWriteTextAsync(
                "a/more/complex/directory/structure/with/an/even/more/complex/file/structure.txt".AsBaselineFilesystemPath()
            );
            await CreateFileAndWriteTextAsync(
                "a/more/complex/directory/structure/with/an/even/more/complex/file/structure.config".AsBaselineFilesystemPath()
            );

            // Act.
            await DirectoryManager.CopyAsync(
                new CopyDirectoryRequest()
                {
                    SourceDirectoryPath =
                        "a/more/complex/directory/structure/".AsBaselineFilesystemPath(),
                    DestinationDirectoryPath = "b/".AsBaselineFilesystemPath()
                }
            );

            // Assert.
            await ExpectFileToExistAsync("b/.keep".AsBaselineFilesystemPath());
            await ExpectFileToExistAsync("b/with/a/nested/file.jpeg".AsBaselineFilesystemPath());
            await ExpectFileToExistAsync(
                "b/with/an/even/more/complex/file/structure.txt".AsBaselineFilesystemPath()
            );
            await ExpectFileToExistAsync(
                "b/with/an/even/more/complex/file/structure.config".AsBaselineFilesystemPath()
            );
        }

        [Fact]
        public async Task It_Copies_A_Large_Directory_Structure_From_One_Location_To_Another()
        {
            // Arrange.
            for (var i = 0; i < 1001; i++)
            {
                await CreateFileAndWriteTextAsync(
                    $"{_sourceDirectory.NormalisedPath}/{i}/.keep".AsBaselineFilesystemPath()
                );
            }

            // Act.
            await DirectoryManager.CopyAsync(
                new CopyDirectoryRequest
                {
                    SourceDirectoryPath = _sourceDirectory,
                    DestinationDirectoryPath = _destinationDirectory
                }
            );

            // Assert.
            for (var i = 0; i < 1001; i++)
            {
                await ExpectFileToExistAsync(
                    $"{_destinationDirectory.NormalisedPath}/{i}/.keep".AsBaselineFilesystemPath()
                );
            }
        }

        [Fact]
        public async Task It_Successfully_Copies_A_Directory_With_A_Root_Path()
        {
            // Arrange.
            ReconfigureManagerInstances(true);

            await CreateFileAndWriteTextAsync(
                $"{_sourceDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath()
            );

            // Act.
            await DirectoryManager.CopyAsync(
                new CopyDirectoryRequest
                {
                    SourceDirectoryPath = _sourceDirectory,
                    DestinationDirectoryPath = _destinationDirectory
                }
            );

            // Assert.
            await ExpectFileToExistAsync(
                $"{_destinationDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath()
            );
        }
    }
}
