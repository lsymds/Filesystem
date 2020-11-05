using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Storio.Tests.Adapters.S3.Integration.Files
{
    public class DeleteFileTests : BaseS3AdapterIntegrationTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_File_Does_Not_Exist()
        {
            Func<Task> func = async () => await FileManager.DeleteAsync(
                new DeleteFileRequest
                {
                    FilePath = "/some/flash/directory/i-do-not-be-delete-for-i-do-not-exist.swf".AsStorioPath()
                }
            );

            await func.Should().ThrowExactlyAsync<FileNotFoundException>();
        }

        [Fact]
        public async Task It_Deletes_A_File()
        {
            var filePath = "/some/flash/directory/i-do-exist-this-time.swf".AsStorioPath();
            
            await FileManager.WriteTextAsync(
                new WriteTextToFileRequest
                {
                    FilePath = filePath,
                    ContentType = "text/plain",
                    TextToWrite = string.Empty
                }
            );

            (await FileExistsAsync(filePath)).Should().BeTrue();

            await FileManager.DeleteAsync(new DeleteFileRequest {FilePath = filePath});
            
            (await FileExistsAsync(filePath)).Should().BeFalse(); 
        }
    }
}
