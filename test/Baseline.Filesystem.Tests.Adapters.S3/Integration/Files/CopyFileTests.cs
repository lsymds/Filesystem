using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class CopyFileTests : BaseS3AdapterIntegrationTest
    {
        private readonly PathRepresentation _sourceFilePath = RandomFilePathRepresentation();
        private readonly PathRepresentation _destinationFilePath = RandomFilePathRepresentation();

        [Fact]
        public async Task It_Throws_An_Exception_If_Source_Path_Does_Not_Exist()
        {
            // Act.
            Func<Task> func = async () =>
                await FileManager.CopyAsync(
                    new CopyFileRequest
                    {
                        SourceFilePath = _sourceFilePath,
                        DestinationFilePath = _destinationFilePath
                    }
                );

            // Assert.
            await func.Should().ThrowExactlyAsync<FileNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_Destination_Path_Already_Exists()
        {
            // Arrange.
            await CreateFileAndWriteTextAsync(_sourceFilePath);
            await CreateFileAndWriteTextAsync(_destinationFilePath);

            // Act.
            Func<Task> func = async () =>
                await FileManager.CopyAsync(
                    new CopyFileRequest
                    {
                        SourceFilePath = _sourceFilePath,
                        DestinationFilePath = _destinationFilePath
                    }
                );

            // Assert.
            await func.Should().ThrowExactlyAsync<FileAlreadyExistsException>();
        }

        [Fact]
        public async Task It_Successfully_Copies_A_File()
        {
            // Arrange.
            await CreateFileAndWriteTextAsync(_sourceFilePath, "[ 1, 2, 3 ]");

            // Act.
            await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = _sourceFilePath,
                    DestinationFilePath = _destinationFilePath
                }
            );

            // Assert.
            await ExpectFileToExistAsync(_sourceFilePath);
            await ExpectFileToExistAsync(_destinationFilePath);

            var destinationContents = await ReadFileAsStringAsync(_destinationFilePath);
            destinationContents.Should().Be("[ 1, 2, 3 ]");
        }

        [Fact]
        public async Task It_Successfully_Copies_A_File_With_A_Root_Path()
        {
            // Arrange.
            ReconfigureManagerInstances(true);

            await CreateFileAndWriteTextAsync(_sourceFilePath, "[ 1, 2, 3 ]");

            // Act.
            await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = _sourceFilePath,
                    DestinationFilePath = _destinationFilePath
                }
            );

            // Assert.
            await ExpectFileToExistAsync(_sourceFilePath);
            await ExpectFileToExistAsync(_destinationFilePath);

            var destinationContents = await ReadFileAsStringAsync(_destinationFilePath);
            destinationContents.Should().Be("[ 1, 2, 3 ]");
        }
    }
}
