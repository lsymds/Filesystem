using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class CopyFileTests : BaseS3AdapterIntegrationTest
    {
        private static readonly PathRepresentation SourceFilePath = "/a-file/in-the-woods/src.json".AsBaselineFilesystemPath();
        private static readonly PathRepresentation DestinationFilePath = "a-file/in-the-woods/dest.json".AsBaselineFilesystemPath();
        
        [Fact]
        public async Task It_Throws_An_Exception_If_Source_Path_Does_Not_Exist()
        {
            Func<Task> func = async () => await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = SourceFilePath,
                    DestinationFilePath = DestinationFilePath
                }
            );
            await func.Should().ThrowExactlyAsync<FileNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_Destination_Path_Already_Exists()
        {
            await FileManager.WriteTextAsync(
                new WriteTextToFileRequest
                {
                    FilePath = SourceFilePath,
                    ContentType = "application/json",
                    TextToWrite = "[ 1, 2, 3 ]"
                }
            );
            
            await FileManager.WriteTextAsync(
                new WriteTextToFileRequest
                {
                    FilePath = DestinationFilePath,
                    ContentType = "application/json",
                    TextToWrite = "[ 1, 2, 3 ]"
                }
            );
            
            Func<Task> func = async () => await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = SourceFilePath,
                    DestinationFilePath = DestinationFilePath
                }
            );
            await func.Should().ThrowExactlyAsync<FileAlreadyExistsException>();
        }

        [Fact]
        public async Task It_Successfully_Copies_A_File()
        {
            await FileManager.WriteTextAsync(
                new WriteTextToFileRequest
                {
                    FilePath = SourceFilePath,
                    ContentType = "application/json",
                    TextToWrite = "[ 1, 2, 3 ]"
                }
            );
            
            await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = SourceFilePath,
                    DestinationFilePath = DestinationFilePath
                }
            );

            (await FileExistsAsync(SourceFilePath)).Should().BeTrue();
            (await FileExistsAsync(DestinationFilePath)).Should().BeTrue();

            var destinationContents = await ReadFileAsStringAsync(DestinationFilePath);
            destinationContents.Should().Be("[ 1, 2, 3 ]");
        }
    }
}
