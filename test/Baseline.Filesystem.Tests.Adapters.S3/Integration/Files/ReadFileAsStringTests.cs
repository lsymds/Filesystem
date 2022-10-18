using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files;

public class ReadFileAsStringTests : BaseS3AdapterIntegrationTest
{
    [Fact]
    public async Task It_Throws_An_Exception_If_File_Does_Not_Exist()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.ReadAsStringAsync(
                new ReadFileAsStringRequest { FilePath = RandomFilePathRepresentation() }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<FileNotFoundException>();
    }

    [Fact]
    public async Task It_Retrieves_File_Contents_If_File_Does_Exist()
    {
        // Arrange.
        var path = RandomFilePathRepresentation();

        await FileManager.WriteTextAsync(
            new WriteTextToFileRequest
            {
                FilePath = path,
                ContentType = "text/plain",
                TextToWrite = "you should check these contents"
            }
        );

        // Act.
        var fileContents = await FileManager.ReadAsStringAsync(
            new ReadFileAsStringRequest { FilePath = path }
        );

        // Assert.
        fileContents.FileContents.Should().Be("you should check these contents");
    }

    [Fact]
    public async Task It_Retrieves_File_Contents_Under_A_Root_Path()
    {
        // Arrange.
        ReconfigureManagerInstances(true);

        var path = RandomFilePathRepresentation();

        await CreateFileAndWriteTextAsync(path, "you should check these contents");

        // Act.
        var fileContents = await FileManager.ReadAsStringAsync(
            new ReadFileAsStringRequest { FilePath = path }
        );

        // Assert.
        fileContents.FileContents.Should().Be("you should check these contents");
    }
}
