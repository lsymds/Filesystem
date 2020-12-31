using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class MoveFileTests : BaseS3AdapterIntegrationTest
    {
        private readonly PathRepresentation _sourceFilePath = RandomFilePath();
        private readonly PathRepresentation _destinationFilePath = RandomFilePath();
        
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Source_File_Does_Not_Exist()
        {
            Func<Task> func = async () => await FileManager.MoveAsync(new MoveFileRequest
            {
                SourceFilePath = _sourceFilePath,
                DestinationFilePath = _destinationFilePath
            });
            await func.Should().ThrowExactlyAsync<FileNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_The_Destination_File_Already_Exists()
        {
            await CreateFileAndWriteTextAsync(_sourceFilePath);
            await CreateFileAndWriteTextAsync(_destinationFilePath);
            
            Func<Task> func = async () => await FileManager.MoveAsync(new MoveFileRequest
            {
                SourceFilePath = _sourceFilePath,
                DestinationFilePath = _destinationFilePath
            });
            await func.Should().ThrowExactlyAsync<FileAlreadyExistsException>();
        }

        [Fact]
        public async Task It_Successfully_Moves_A_File_From_One_Place_To_Another()
        {
            await CreateFileAndWriteTextAsync(_sourceFilePath, "abc");

            await FileManager.MoveAsync(new MoveFileRequest
            {
                SourceFilePath = _sourceFilePath,
                DestinationFilePath = _destinationFilePath
            });

            var fileContents = await FileManager.ReadAsStringAsync(new ReadFileAsStringRequest
            {
                FilePath = _destinationFilePath
            });
            fileContents.Should().Be("abc");
        }
    }
}
