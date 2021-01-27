using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Directories
{
    public class MoveDirectoryTests : BaseS3AdapterIntegrationTest
    {
        private readonly PathRepresentation _sourceDirectory = RandomDirectoryPathRepresentation();
        private readonly PathRepresentation _destinationDirectory = RandomDirectoryPathRepresentation();
        
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Source_Directory_Does_Not_Exist()
        {
            Func<Task> func = async () => await DirectoryManager.MoveAsync(new MoveDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            });
            await func.Should().ThrowExactlyAsync<DirectoryNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Destination_Directory_Exists()
        {
            await CreateFileAndWriteTextAsync($"{_sourceDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath());
            await CreateFileAndWriteTextAsync($"{_destinationDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath());
            
            Func<Task> func = async () => await DirectoryManager.MoveAsync(new MoveDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            });
            await func.Should().ThrowExactlyAsync<DirectoryAlreadyExistsException>();
        }

        [Fact]
        public async Task It_Successfully_Moves_A_Simple_Directory_Structure_From_One_Location_To_Another()
        {
            var originalFirstFilePath = $"{_sourceDirectory.NormalisedPath}/a/b.txt".AsBaselineFilesystemPath();
            var originalSecondFilePath = $"{_sourceDirectory.NormalisedPath}/a/b/c.txt".AsBaselineFilesystemPath();
            
            await CreateFileAndWriteTextAsync(originalFirstFilePath);
            await CreateFileAndWriteTextAsync(originalFirstFilePath);

            await DirectoryManager.MoveAsync(new MoveDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            });

            (await FileExistsAsync(originalFirstFilePath)).Should().BeFalse();
            (await FileExistsAsync(originalSecondFilePath)).Should().BeFalse();
        }

        [Fact]
        public async Task It_Moves_A_More_Complex_Directory_Structure_From_One_Location_To_Another()
        {
            
        }

        [Fact]
        public async Task It_Moves_A_Large_Directory_Structure_From_One_Location_To_Another()
        {
            
        }

        [Fact]
        public async Task It_Moves_A_Directory_Structure_With_A_Root_Path_From_One_Location_To_Another()
        {
            
        }
    }
}
