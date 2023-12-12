using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Adapters.Files;

public class MoveFileTests : BaseIntegrationTest
{
    private readonly PathRepresentation _sourceFilePath =
        TestUtilities.RandomFilePathRepresentation();
    private readonly PathRepresentation _destinationFilePath =
        TestUtilities.RandomFilePathRepresentation();

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Throws_An_Exception_When_The_Source_File_Does_Not_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

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

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Throws_An_Exception_When_The_Destination_File_Already_Exists(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        await TestAdapter.CreateFileAndWriteTextAsync(_sourceFilePath);
        await TestAdapter.CreateFileAndWriteTextAsync(_destinationFilePath);

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

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Moves_A_File_From_One_Place_To_Another(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        await TestAdapter.CreateFileAndWriteTextAsync(_sourceFilePath, "abc");

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

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Moves_A_File_Under_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        await TestAdapter.CreateFileAndWriteTextAsync(_sourceFilePath, "abc");

        // Act.
        await FileManager.MoveAsync(
            new MoveFileRequest
            {
                SourceFilePath = _sourceFilePath,
                DestinationFilePath = _destinationFilePath
            }
        );

        // Assert.
        var fileContents = await TestAdapter.ReadFileAsStringAsync(_destinationFilePath);
        fileContents.Should().Be("abc");
    }
}
