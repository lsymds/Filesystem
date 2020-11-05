using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Storio.Tests.Adapters.S3.Integration.Files
{
    public class ReadFileAsStringTests : BaseS3AdapterIntegrationTest
    {
        public ReadFileAsStringTests() : base(true)
        {
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_File_Does_Not_Exist()
        {
            Func<Task> func = async () => await ReadFileAsStringAsync("ab-i-dont-exist".AsStorioPath());
            await func.Should().ThrowExactlyAsync<FileNotFoundException>();
        }

        [Fact]
        public async Task It_Retrieves_File_Contents_If_File_Does_Exist()
        {
            await FileManager.WriteTextAsync(
                new WriteTextToFileRequest
                {
                    FilePath = "i-do-exist-this-time.txt".AsStorioPath(),
                    ContentType = "text/plain",
                    TextToWrite = "you should check these contents"
                }
            );

            var fileContents = await ReadFileAsStringAsync("i-do-exist-this-time.txt".AsStorioPath());
            fileContents.Should().Be("you should check these contents");
        }
    }
}
