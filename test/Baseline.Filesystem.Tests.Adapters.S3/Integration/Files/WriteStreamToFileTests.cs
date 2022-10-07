using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class WriteStreamToFileTests : BaseS3AdapterIntegrationTest
    {
        private readonly PathRepresentation _filePath = RandomFilePathRepresentation();
        private readonly Stream _stream = new MemoryStream(
            Encoding.UTF8.GetBytes("hello stream my old friend")
        );

        [Fact]
        public async Task It_Successfully_Writes_A_Stream_To_A_File()
        {
            // Arrange.
            await ExpectFileNotToExistAsync(_filePath);

            // Act.
            await FileManager.WriteStreamAsync(
                new WriteStreamToFileRequest
                {
                    FilePath = _filePath,
                    ContentType = "application/json",
                    Stream = _stream
                }
            );

            // Assert.
            var fileContents = await ReadFileAsStringAsync(_filePath);
            fileContents.Should().Be("hello stream my old friend");
            _stream.CanRead.Should().BeTrue();
        }

        [Fact]
        public async Task It_Successfully_Restarts_The_Stream_If_It_Has_Already_Been_Read()
        {
            // Arrange.
            _stream.Seek(5, SeekOrigin.Begin);

            await ExpectFileNotToExistAsync(_filePath);

            // Act.
            await FileManager.WriteStreamAsync(
                new WriteStreamToFileRequest
                {
                    FilePath = _filePath,
                    ContentType = "application/json",
                    Stream = _stream
                }
            );

            // Assert.
            var fileContents = await ReadFileAsStringAsync(_filePath);
            fileContents.Should().Be("hello stream my old friend");
            _stream.CanRead.Should().BeTrue();
        }

        [Fact]
        public async Task It_Successfully_Writes_A_Stream_Under_A_Root_Path_In_S3()
        {
            // Arrange.
            ReconfigureManagerInstances(true);

            await ExpectFileNotToExistAsync(_filePath);

            // Act.
            await FileManager.WriteStreamAsync(
                new WriteStreamToFileRequest
                {
                    FilePath = _filePath,
                    ContentType = "application/json",
                    Stream = _stream
                }
            );

            // Assert.
            var fileContents = await ReadFileAsStringAsync(_filePath);
            fileContents.Should().Be("hello stream my old friend");
            _stream.CanRead.Should().BeTrue();
        }
    }
}
