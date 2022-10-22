using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.Directories;

public class CreateDirectoryTests : BaseIntegrationTest
{
    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Throws_An_Exception_If_The_Directory_Already_Exists(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var directory = TestUtilities.RandomDirectoryPathRepresentation();
        var pathWithDirectory = TestUtilities.RandomFilePathRepresentationWithPrefix(
            directory.OriginalPath
        );

        await TestAdapter.CreateFileAndWriteTextAsync(pathWithDirectory);

        // Act.
        Func<Task> func = async () =>
            await DirectoryManager.CreateAsync(
                new CreateDirectoryRequest { DirectoryPath = directory }
            );

        // Assert.
        await func.Should().ThrowAsync<DirectoryAlreadyExistsException>();
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Creates_The_Directory(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var directory = TestUtilities.RandomDirectoryPathRepresentation();

        // Act.
        var response = await DirectoryManager.CreateAsync(
            new CreateDirectoryRequest { DirectoryPath = directory }
        );

        // Assert.
        await ExpectDirectoryToExistAsync(directory);
        response.Directory.Path.Should().BeEquivalentTo(directory);
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Creates_The_Directory_Under_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var directory = TestUtilities.RandomDirectoryPathRepresentation();

        // Act.
        var response = await DirectoryManager.CreateAsync(
            new CreateDirectoryRequest { DirectoryPath = directory }
        );

        // Assert.
        await ExpectDirectoryToExistAsync(directory);
        response.Directory.Path
            .Should()
            .BeEquivalentTo(directory, x => x.Excluding(y => y.GetPathTree));
    }
}
