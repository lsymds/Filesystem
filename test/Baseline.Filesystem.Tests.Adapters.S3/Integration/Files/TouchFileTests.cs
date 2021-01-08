using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class TouchFileTests : BaseS3AdapterIntegrationTest
    {
        [Fact]
        public async Task It_Successfully_Touches_A_File_In_S3()
        {
            var path = RandomFilePathRepresentation();

            await CreateFileAndWriteTextAsync(path);

            var fileInS3 = await S3Client.GetObjectAsync(
                GeneratedBucketName, 
                path.NormalisedPath
            );
            fileInS3.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            
            var contents = await new StreamReader(fileInS3.ResponseStream).ReadToEndAsync();
            contents.Should().BeEmpty();
        }
    }
}
