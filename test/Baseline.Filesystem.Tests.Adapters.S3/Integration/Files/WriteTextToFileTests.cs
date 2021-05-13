using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class WriteTextToFileTests : BaseS3AdapterIntegrationTest
    {
        [Fact]
        public async Task It_Successfully_Writes_A_Simple_File_To_S3()
        {
            // Arrange.
            var path = RandomFilePathRepresentation();
            
            // Act.
            await FileManager.WriteTextAsync(new WriteTextToFileRequest
            {
                ContentType = "text/plain",
                FilePath = path,
                TextToWrite = "it-successfully-writes-simple-file-to-s3"
            });
            
            // Assert.
            await ExpectFileToExistAsync(path);
            (await ReadFileAsStringAsync(path)).Should().Be("it-successfully-writes-simple-file-to-s3");
        }

        [Fact]
        public async Task It_Successfully_Writes_A_Simple_File_Under_A_Root_Path_To_S3()
        {
            // Arrange.
            ReconfigureManagerInstances(true);
            
            var path = RandomFilePathRepresentation();
            
            // Act.
            await FileManager.WriteTextAsync(new WriteTextToFileRequest
            {
                ContentType = "text/plain",
                FilePath = path,
                TextToWrite = "it-successfully-writes-simple-file-to-s3"
            });
            
            // Assert.
            await ExpectFileToExistAsync(path);
            (await ReadFileAsStringAsync(path)).Should().Be("it-successfully-writes-simple-file-to-s3");
        }
    }
}
