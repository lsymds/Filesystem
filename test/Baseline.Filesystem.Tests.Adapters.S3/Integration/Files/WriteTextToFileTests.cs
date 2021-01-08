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
            var path = RandomFilePathRepresentation();
            
            await FileManager.WriteTextAsync(new WriteTextToFileRequest
            {
                ContentType = "text/plain",
                FilePath = path,
                TextToWrite = "it-successfully-writes-simple-file-to-s3"
            });

            var fileInS3 = await S3Client.GetObjectAsync(
                GeneratedBucketName, 
                path.NormalisedPath
            );
            fileInS3.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            
            var contents = await new StreamReader(fileInS3.ResponseStream).ReadToEndAsync();
            contents.Should().Be("it-successfully-writes-simple-file-to-s3");
        }
    }
}
