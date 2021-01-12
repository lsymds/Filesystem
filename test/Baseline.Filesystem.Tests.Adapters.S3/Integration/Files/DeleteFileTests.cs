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
                    FilePath = RandomFilePathRepresentation()
                }
            );

            await func.Should().ThrowExactlyAsync<FileNotFoundException>();
        }

        [Fact]
        public async Task It_Deletes_A_File()
        {
            var filePath = RandomFilePathRepresentation();

            await CreateFileAndWriteTextAsync(filePath);

            (await FileExistsAsync(filePath)).Should().BeTrue();

            await FileManager.DeleteAsync(new DeleteFileRequest {FilePath = filePath});
            
            (await FileExistsAsync(filePath)).Should().BeFalse(); 
        }

        [Fact]
        public async Task It_Deletes_A_File_With_A_Root_Path()
        {
            ReconfigureManagerInstances(true);
            
            var filePath = RandomFilePathRepresentation();

            await CreateFileAndWriteTextAsync(filePath);

            (await FileExistsAsync(filePath)).Should().BeTrue();

            await FileManager.DeleteAsync(new DeleteFileRequest {FilePath = filePath});
            
            (await FileExistsAsync(filePath)).Should().BeFalse(); 
        }
    }
}
