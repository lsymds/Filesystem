using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class CopyFileTests : BaseS3AdapterIntegrationTest
    {
        private readonly PathRepresentation _sourceFilePath = RandomFilePath().AsBaselineFilesystemPath();
        private readonly PathRepresentation _destinationFilePath = RandomFilePath().AsBaselineFilesystemPath();
        
        [Fact]
        public async Task It_Throws_An_Exception_If_Source_Path_Does_Not_Exist()
        {
            Func<Task> func = async () => await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = _sourceFilePath,
                    DestinationFilePath = _destinationFilePath
                }
            );
            await func.Should().ThrowExactlyAsync<FileNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_Destination_Path_Already_Exists()
        {
            await CreateFileAndWriteTextAsync(_sourceFilePath);
            await CreateFileAndWriteTextAsync(_destinationFilePath);
            
            Func<Task> func = async () => await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = _sourceFilePath,
                    DestinationFilePath = _destinationFilePath
                }
            );
            await func.Should().ThrowExactlyAsync<FileAlreadyExistsException>();
        }

        [Fact]
        public async Task It_Successfully_Copies_A_File()
        {
            await CreateFileAndWriteTextAsync(_sourceFilePath, "[ 1, 2, 3 ]");
            
            await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = _sourceFilePath,
                    DestinationFilePath = _destinationFilePath
                }
            );

            (await FileExistsAsync(_sourceFilePath)).Should().BeTrue();
            (await FileExistsAsync(_destinationFilePath)).Should().BeTrue();

            var destinationContents = await ReadFileAsStringAsync(_destinationFilePath);
            destinationContents.Should().Be("[ 1, 2, 3 ]");
        }
    }
}
