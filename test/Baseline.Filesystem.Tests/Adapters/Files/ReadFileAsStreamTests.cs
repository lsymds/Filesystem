using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.Files;

public class ReadFileAsStreamTests : BaseIntegrationTest
{
    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Throws_An_Exception_If_File_Does_Not_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        // Act.
        Func<Task> func = async () =>
            await FileManager.ReadAsStreamAsync(
                new ReadFileAsStreamRequest
                {
                    FilePath = TestUtilities.RandomFilePathRepresentation()
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<FileNotFoundException>();
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Retrieves_File_Contents_Stream_If_File_Does_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var path = TestUtilities.RandomFilePathRepresentation();

        await FileManager.WriteTextAsync(
            new WriteTextToFileRequest
            {
                FilePath = path,
                ContentType = "text/plain",
                TextToWrite = "you should check these contents"
            }
        );

        // Act.
        var fileContents = await FileManager.ReadAsStreamAsync(
            new ReadFileAsStreamRequest { FilePath = path }
        );

        // Assert.
        (await new StreamReader(fileContents.FileContents).ReadToEndAsync())
            .Should()
            .Be("you should check these contents");
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Retrieves_File_Stream_Contents_Under_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var path = TestUtilities.RandomFilePathRepresentation();

        await TestAdapter.CreateFileAndWriteTextAsync(path, "you should check these contents");

        // Act.
        var fileContents = await FileManager.ReadAsStreamAsync(
            new ReadFileAsStreamRequest { FilePath = path }
        );

        // Assert.
        (await new StreamReader(fileContents.FileContents).ReadToEndAsync())
            .Should()
            .Be("you should check these contents");
    }
}
