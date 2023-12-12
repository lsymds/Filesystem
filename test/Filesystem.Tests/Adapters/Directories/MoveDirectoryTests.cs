using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Adapters.Directories;

public class MoveDirectoryTests : BaseIntegrationTest
{
    private readonly PathRepresentation _sourceDirectory =
        TestUtilities.RandomDirectoryPathRepresentation();
    private readonly PathRepresentation _destinationDirectory =
        TestUtilities.RandomDirectoryPathRepresentation();

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Throws_An_Exception_If_The_Source_Directory_Does_Not_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        // Act.
        Func<Task> func = async () =>
            await DirectoryManager.MoveAsync(
                new MoveDirectoryRequest
                {
                    SourceDirectoryPath = _sourceDirectory,
                    DestinationDirectoryPath = _destinationDirectory
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<DirectoryNotFoundException>();
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Throws_An_Exception_If_The_Destination_Directory_Exists(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        await TestAdapter.CreateFileAndWriteTextAsync(
            $"{_sourceDirectory.NormalisedPath}/.keep".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            $"{_destinationDirectory.NormalisedPath}/.keep".AsFilesystemPath()
        );

        // Act.
        Func<Task> func = async () =>
            await DirectoryManager.MoveAsync(
                new MoveDirectoryRequest
                {
                    SourceDirectoryPath = _sourceDirectory,
                    DestinationDirectoryPath = _destinationDirectory
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<DirectoryAlreadyExistsException>();
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Moves_A_Simple_Directory_Structure_From_One_Location_To_Another(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var originalFirstFilePath =
            $"{_sourceDirectory.NormalisedPath}/a/b.txt".AsFilesystemPath();
        var originalSecondFilePath =
            $"{_sourceDirectory.NormalisedPath}/a/b/c.txt".AsFilesystemPath();

        await TestAdapter.CreateFileAndWriteTextAsync(originalFirstFilePath);
        await TestAdapter.CreateFileAndWriteTextAsync(originalSecondFilePath);

        // Act.
        await DirectoryManager.MoveAsync(
            new MoveDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            }
        );

        // Assert.
        await ExpectDirectoryNotToExistAsync(_sourceDirectory);
        await ExpectFileNotToExistAsync(originalSecondFilePath);
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Moves_A_More_Complex_Directory_Structure_From_One_Location_To_Another(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var files = new[]
        {
            "a/b/c/.keep",
            "a/b/c/.keeps",
            "a/b/c/d/e/f/g/.keep",
            "a/b/c/d/e/f/g/.keeps",
            "a/b/c/d/v/foo.keep",
            "a/b/c/d/v/foo.keeps",
            "a/b/c/d/e/f/a/.keep",
            "a/b/c/d/e/f/a/.keeps"
        };

        foreach (var file in files)
        {
            await TestAdapter.CreateFileAndWriteTextAsync(
                $"{_sourceDirectory.NormalisedPath}/{file}".AsFilesystemPath()
            );
        }

        // Act.
        await DirectoryManager.MoveAsync(
            new MoveDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            }
        );

        // Assert.
        await ExpectDirectoryNotToExistAsync(_sourceDirectory);
        foreach (var file in files)
        {
            await ExpectFileToExistAsync(
                $"{_destinationDirectory.NormalisedPath}/{file}".AsFilesystemPath()
            );
        }
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Moves_A_Large_Directory_Structure_From_One_Location_To_Another(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        for (var i = 0; i < 1001; i++)
        {
            await TestAdapter.CreateFileAndWriteTextAsync(
                $"{_sourceDirectory.NormalisedPath}/{i}/.keep".AsFilesystemPath()
            );
        }

        // Act.
        await DirectoryManager.MoveAsync(
            new MoveDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            }
        );

        // Assert.
        await ExpectDirectoryNotToExistAsync(_sourceDirectory);
        for (var i = 0; i < 1001; i++)
        {
            await ExpectFileToExistAsync(
                $"{_destinationDirectory.NormalisedPath}/{i}/.keep".AsFilesystemPath()
            );
        }
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Moves_A_Directory_Structure_With_A_Root_Path_From_One_Location_To_Another(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var originalFirstFilePath =
            $"{_sourceDirectory.NormalisedPath}/a/b.txt".AsFilesystemPath();
        var originalSecondFilePath =
            $"{_sourceDirectory.NormalisedPath}/a/b/c.txt".AsFilesystemPath();

        await TestAdapter.CreateFileAndWriteTextAsync(originalFirstFilePath);
        await TestAdapter.CreateFileAndWriteTextAsync(originalSecondFilePath);

        // Act.
        await DirectoryManager.MoveAsync(
            new MoveDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            }
        );

        // Assert.
        await ExpectDirectoryNotToExistAsync(_sourceDirectory);
        await ExpectFileNotToExistAsync(originalSecondFilePath);
    }
}
