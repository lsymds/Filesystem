using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Directories
{
    public class MoveDirectoryTests : BaseS3AdapterIntegrationTest
    {
        private readonly PathRepresentation _sourceDirectory = RandomDirectoryPathRepresentation();
        private readonly PathRepresentation _destinationDirectory =
            RandomDirectoryPathRepresentation();

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Source_Directory_Does_Not_Exist()
        {
            // Act.
            Func<Task> func = async () =>
                await DirectoryManager.MoveAsync(
                    new MoveDirectoryRequest
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
                await DirectoryManager.MoveAsync(
                    new MoveDirectoryRequest
                    {
                        SourceDirectoryPath = _sourceDirectory,
                        DestinationDirectoryPath = _destinationDirectory
                    }
                );

            // Assert.
            await func.Should().ThrowExactlyAsync<DirectoryAlreadyExistsException>();
        }

        [Fact]
        public async Task It_Successfully_Moves_A_Simple_Directory_Structure_From_One_Location_To_Another()
        {
            // Arrange.
            var originalFirstFilePath =
                $"{_sourceDirectory.NormalisedPath}/a/b.txt".AsBaselineFilesystemPath();
            var originalSecondFilePath =
                $"{_sourceDirectory.NormalisedPath}/a/b/c.txt".AsBaselineFilesystemPath();

            await CreateFileAndWriteTextAsync(originalFirstFilePath);
            await CreateFileAndWriteTextAsync(originalSecondFilePath);

            // Act.
            await DirectoryManager.MoveAsync(
                new MoveDirectoryRequest
                {
                    SourceDirectoryPath = _sourceDirectory,
                    DestinationDirectoryPath = _destinationDirectory
                }
            );

            // Assert.
            await ExpectDirectoryNotToExistAsync(_sourceDirectory);
            await ExpectFileNotToExistAsync(originalSecondFilePath);
        }

        [Fact]
        public async Task It_Moves_A_More_Complex_Directory_Structure_From_One_Location_To_Another()
        {
            // Arrange.
            var files = new[]
            {
                "a/b/c/.keep",
                "a/b/c/.keeps",
                "a/b/c/d/e/f/g/.keep",
                "a/b/c/d/e/f/g/.keeps",
                "a/b/c/d/v/foo.keep",
                "a/b/c/d/v/foo.keeps",
                "a/b/c/d/e/f/a/.keep",
                "a/b/c/d/e/f/a/.keeps"
            };

            foreach (var file in files)
            {
                await CreateFileAndWriteTextAsync(
                    $"{_sourceDirectory.OriginalPath}/{file}".AsBaselineFilesystemPath()
                );
            }

            // Act.
            await DirectoryManager.MoveAsync(
                new MoveDirectoryRequest
                {
                    SourceDirectoryPath = _sourceDirectory,
                    DestinationDirectoryPath = _destinationDirectory
                }
            );

            // Assert.
            await ExpectDirectoryNotToExistAsync(_sourceDirectory);
            foreach (var file in files)
            {
                await ExpectFileToExistAsync(
                    $"{_destinationDirectory.OriginalPath}/{file}".AsBaselineFilesystemPath()
                );
            }
        }

        [Fact]
        public async Task It_Moves_A_Large_Directory_Structure_From_One_Location_To_Another()
        {
            // Arrange.
            for (var i = 0; i < 1001; i++)
            {
                await CreateFileAndWriteTextAsync(
                    $"{_sourceDirectory.NormalisedPath}/{i}/.keep".AsBaselineFilesystemPath()
                );
            }

            // Act.
            await DirectoryManager.MoveAsync(
                new MoveDirectoryRequest
                {
                    SourceDirectoryPath = _sourceDirectory,
                    DestinationDirectoryPath = _destinationDirectory
                }
            );

            // Assert.
            await ExpectDirectoryNotToExistAsync(_sourceDirectory);
            for (var i = 0; i < 1001; i++)
            {
                await ExpectFileToExistAsync(
                    $"{_destinationDirectory.NormalisedPath}/{i}/.keep".AsBaselineFilesystemPath()
                );
            }
        }

        [Fact]
        public async Task It_Moves_A_Directory_Structure_With_A_Root_Path_From_One_Location_To_Another()
        {
            // Arrange.
            ReconfigureManagerInstances(true);

            var originalFirstFilePath =
                $"{_sourceDirectory.NormalisedPath}/a/b.txt".AsBaselineFilesystemPath();
            var originalSecondFilePath =
                $"{_sourceDirectory.NormalisedPath}/a/b/c.txt".AsBaselineFilesystemPath();

            await CreateFileAndWriteTextAsync(originalFirstFilePath);
            await CreateFileAndWriteTextAsync(originalSecondFilePath);

            // Act.
            await DirectoryManager.MoveAsync(
                new MoveDirectoryRequest
                {
                    SourceDirectoryPath = _sourceDirectory,
                    DestinationDirectoryPath = _destinationDirectory
                }
            );

            // Assert.
            await ExpectDirectoryNotToExistAsync(_sourceDirectory);
            await ExpectFileNotToExistAsync(originalSecondFilePath);
        }
    }
}
