using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Files;

public class MoveFileTests : BaseS3AdapterIntegrationTest
{
    private readonly PathRepresentation _sourceFilePath = RandomFilePathRepresentation();
    private readonly PathRepresentation _destinationFilePath = RandomFilePathRepresentation();

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Source_File_Does_Not_Exist()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.MoveAsync(
                new MoveFileRequest
                {
                    SourceFilePath = _sourceFilePath,
                    DestinationFilePath = _destinationFilePath
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<FileNotFoundException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Destination_File_Already_Exists()
    {
        // Arrange.
        await CreateFileAndWriteTextAsync(_sourceFilePath);
        await CreateFileAndWriteTextAsync(_destinationFilePath);

        // Act.
        Func<Task> func = async () =>
            await FileManager.MoveAsync(
                new MoveFileRequest
                {
                    SourceFilePath = _sourceFilePath,
                    DestinationFilePath = _destinationFilePath
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<FileAlreadyExistsException>();
    }

    [Fact]
    public async Task It_Successfully_Moves_A_File_From_One_Place_To_Another()
    {
        // Arrange.
        await CreateFileAndWriteTextAsync(_sourceFilePath, "abc");

        await FileManager.MoveAsync(
            new MoveFileRequest
            {
                SourceFilePath = _sourceFilePath,
                DestinationFilePath = _destinationFilePath
            }
        );

        // Act.
        var fileContents = await FileManager.ReadAsStringAsync(
            new ReadFileAsStringRequest { FilePath = _destinationFilePath }
        );

        // Assert.
        fileContents.FileContents.Should().Be("abc");
    }

    [Fact]
    public async Task It_Successfully_Moves_A_File_Under_A_Root_Path()
    {
        // Arrange.
        ReconfigureManagerInstances(true);

        await CreateFileAndWriteTextAsync(_sourceFilePath, "abc");

        // Act.
        await FileManager.MoveAsync(
            new MoveFileRequest
            {
                SourceFilePath = _sourceFilePath,
                DestinationFilePath = _destinationFilePath
            }
        );

        // Assert.
        var fileContents = await ReadFileAsStringAsync(_destinationFilePath);
        fileContents.Should().Be("abc");
    }
}
