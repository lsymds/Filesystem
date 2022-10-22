using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.Files;

public class ReadFileAsStringTests : BaseIntegrationTest
{
    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Throws_An_Exception_If_File_Does_Not_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        // Act.
        Func<Task> func = async () =>
            await FileManager.ReadAsStringAsync(
                new ReadFileAsStringRequest
                {
                    FilePath = TestUtilities.RandomFilePathRepresentation()
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<FileNotFoundException>();
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Retrieves_File_Contents_If_File_Does_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var path = TestUtilities.RandomFilePathRepresentation();

        await TestAdapter.CreateFileAndWriteTextAsync(path, "you should check these contents");

        // Act.
        var fileContents = await FileManager.ReadAsStringAsync(
            new ReadFileAsStringRequest { FilePath = path }
        );

        // Assert.
        fileContents.FileContents.Should().Be("you should check these contents");
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Retrieves_File_Contents_Under_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var path = TestUtilities.RandomFilePathRepresentation();

        await TestAdapter.CreateFileAndWriteTextAsync(path, "you should check these contents");

        // Act.
        var fileContents = await FileManager.ReadAsStringAsync(
            new ReadFileAsStringRequest { FilePath = path }
        );

        // Assert.
        fileContents.FileContents.Should().Be("you should check these contents");
    }
}
