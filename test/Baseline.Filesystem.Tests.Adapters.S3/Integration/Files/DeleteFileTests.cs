using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class DeleteFileTests : BaseS3AdapterIntegrationTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_File_Does_Not_Exist()
        {
            Func<Task> func = async () => await FileManager.DeleteAsync(
                new DeleteFileRequest
                {
                    FilePath = RandomFilePath().AsBaselineFilesystemPath()
                }
            );

            await func.Should().ThrowExactlyAsync<FileNotFoundException>();
        }

        [Fact]
        public async Task It_Deletes_A_File()
        {
            var filePath = RandomFilePath().AsBaselineFilesystemPath();

            await CreateFileAndWriteTextAsync(filePath);

            (await FileExistsAsync(filePath)).Should().BeTrue();

            await FileManager.DeleteAsync(new DeleteFileRequest {FilePath = filePath});
            
            (await FileExistsAsync(filePath)).Should().BeFalse(); 
        }
    }
}
