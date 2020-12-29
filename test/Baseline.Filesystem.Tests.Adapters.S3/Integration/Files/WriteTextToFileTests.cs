using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class WriteTextToFileTests : BaseS3AdapterIntegrationTest
    {
        [Fact]
        public async Task It_Successfully_Writes_Simple_File_To_S3()
        {
            await FileManager.WriteTextAsync(new WriteTextToFileRequest
            {
                ContentType = "text/plain",
                FilePath = "/tests/simple-text-to-file/simple-file.txt".AsBaselineFilesystemPath(),
                TextToWrite = "it-successfully-writes-simple-file-to-s3"
            });

            var fileInS3 = await S3Client.GetObjectAsync(
                GeneratedBucketName, 
                "tests/simple-text-to-file/simple-file.txt"
            );
            fileInS3.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            
            var contents = await new StreamReader(fileInS3.ResponseStream).ReadToEndAsync();
            contents.Should().Be("it-successfully-writes-simple-file-to-s3");
        }
    }
}
