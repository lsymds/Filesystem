using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class ReadFileAsStringTests : BaseS3AdapterIntegrationTest
    {
        public ReadFileAsStringTests() : base(true)
        {
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_File_Does_Not_Exist()
        {
            Func<Task> func = async () => await ReadFileAsStringAsync(RandomFilePath());
            await func.Should().ThrowExactlyAsync<FileNotFoundException>();
        }

        [Fact]
        public async Task It_Retrieves_File_Contents_If_File_Does_Exist()
        {
            var path = RandomFilePath();
            
            await FileManager.WriteTextAsync(
                new WriteTextToFileRequest
                {
                    FilePath = path,
                    ContentType = "text/plain",
                    TextToWrite = "you should check these contents"
                }
            );

            var fileContents = await ReadFileAsStringAsync(path);
            fileContents.Should().Be("you should check these contents");
        }
    }
}
