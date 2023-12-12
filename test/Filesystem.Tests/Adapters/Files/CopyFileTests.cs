using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Adapters.Files;

public class CopyFileTests : BaseIntegrationTest
{
    private readonly PathRepresentation _sourceFilePath =
        TestUtilities.RandomFilePathRepresentation();
    private readonly PathRepresentation _destinationFilePath =
        TestUtilities.RandomFilePathRepresentation();

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Throws_An_Exception_If_Source_Path_Does_Not_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        // Act.
        Func<Task> func = async () =>
            await FileManager.CopyAsync(
                new CopyFileRequest
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
    public async Task It_Throws_An_Exception_If_Destination_Path_Already_Exists(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        await TestAdapter.CreateFileAndWriteTextAsync(_sourceFilePath);
        await TestAdapter.CreateFileAndWriteTextAsync(_destinationFilePath);

        // Act.
        Func<Task> func = async () =>
            await FileManager.CopyAsync(
                new CopyFileRequest
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
    public async Task It_Successfully_Copies_A_File(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        await TestAdapter.CreateFileAndWriteTextAsync(_sourceFilePath, "[ 1, 2, 3 ]");

        // Act.
        await FileManager.CopyAsync(
            new CopyFileRequest
            {
                SourceFilePath = _sourceFilePath,
                DestinationFilePath = _destinationFilePath
            }
        );

        // Assert.
        await ExpectFileToExistAsync(_sourceFilePath);
        await ExpectFileToExistAsync(_destinationFilePath);

        var destinationContents = await TestAdapter.ReadFileAsStringAsync(_destinationFilePath);
        destinationContents.Should().Be("[ 1, 2, 3 ]");
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Copies_A_File_With_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        await TestAdapter.CreateFileAndWriteTextAsync(_sourceFilePath, "[ 1, 2, 3 ]");

        // Act.
        await FileManager.CopyAsync(
            new CopyFileRequest
            {
                SourceFilePath = _sourceFilePath,
                DestinationFilePath = _destinationFilePath
            }
        );

        // Assert.
        await ExpectFileToExistAsync(_sourceFilePath);
        await ExpectFileToExistAsync(_destinationFilePath);

        var destinationContents = await TestAdapter.ReadFileAsStringAsync(_destinationFilePath);
        destinationContents.Should().Be("[ 1, 2, 3 ]");
    }
}
