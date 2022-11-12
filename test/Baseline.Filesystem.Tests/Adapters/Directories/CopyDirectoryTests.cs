using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.Directories;

public class CopyDirectoryTests : BaseIntegrationTest
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
            await DirectoryManager.CopyAsync(
                new CopyDirectoryRequest
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
            $"{_sourceDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            $"{_destinationDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath()
        );

        // Act.
        Func<Task> func = async () =>
            await DirectoryManager.CopyAsync(
                new CopyDirectoryRequest
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
    public async Task It_Copies_A_Simple_Directory_Structure_From_One_Location_To_Another(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var files = new[]
        {
            TestUtilities.RandomFilePathRepresentationWithPrefix(_sourceDirectory.NormalisedPath),
            TestUtilities.RandomFilePathRepresentationWithPrefix(_sourceDirectory.NormalisedPath),
            TestUtilities.RandomFilePathRepresentationWithPrefix(_sourceDirectory.NormalisedPath),
            TestUtilities.RandomFilePathRepresentationWithPrefix(_sourceDirectory.NormalisedPath),
            TestUtilities.RandomFilePathRepresentationWithPrefix(_sourceDirectory.NormalisedPath),
        };

        foreach (var file in files)
        {
            await TestAdapter.CreateFileAndWriteTextAsync(file);
        }

        // Act.
        await DirectoryManager.CopyAsync(
            new CopyDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            }
        );

        // Assert.
        foreach (var file in files)
        {
            var newDirectoryPath = file.NormalisedPath.Replace(
                _sourceDirectory.NormalisedPath,
                _destinationDirectory.NormalisedPath
            );

            await ExpectFileToExistAsync(newDirectoryPath.AsBaselineFilesystemPath());
        }
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Copies_A_Directory_Structure_With_A_Repeated_Directory_Name_Correctly(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var originalDirectory = "cheese/".AsBaselineFilesystemPath();
        var originalFile =
            "cheese/cheese/more-cheese/my-favourite-cheesestring.jpeg".AsBaselineFilesystemPath();

        await TestAdapter.CreateFileAndWriteTextAsync(originalFile);

        // Act.
        await DirectoryManager.CopyAsync(
            new CopyDirectoryRequest
            {
                SourceDirectoryPath = originalDirectory,
                DestinationDirectoryPath = _destinationDirectory
            }
        );

        // Assert.
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/cheese/more-cheese/my-favourite-cheesestring.jpeg".AsBaselineFilesystemPath()
        );
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Copies_A_More_Complex_Directory_Structure_From_One_Location_To_Another(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/more/complex/directory/structure/.keep".AsBaselineFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/more/complex/directory/structure/with/a/nested/file.jpeg".AsBaselineFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/more/complex/directory/structure/with/an/even/more/complex/file/structure.txt".AsBaselineFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/more/complex/directory/structure/with/an/even/more/complex/file/structure.config".AsBaselineFilesystemPath()
        );

        // Act.
        await DirectoryManager.CopyAsync(
            new CopyDirectoryRequest()
            {
                SourceDirectoryPath =
                    "a/more/complex/directory/structure/".AsBaselineFilesystemPath(),
                DestinationDirectoryPath = _destinationDirectory
            }
        );

        // Assert.
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath()
        );
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/with/a/nested/file.jpeg".AsBaselineFilesystemPath()
        );
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/with/an/even/more/complex/file/structure.txt".AsBaselineFilesystemPath()
        );
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/with/an/even/more/complex/file/structure.config".AsBaselineFilesystemPath()
        );
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Copies_A_Large_Directory_Structure_From_One_Location_To_Another(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        for (var i = 0; i < 1001; i++)
        {
            await TestAdapter.CreateFileAndWriteTextAsync(
                $"{_sourceDirectory.NormalisedPath}/{i}/.keep".AsBaselineFilesystemPath()
            );
        }

        // Act.
        await DirectoryManager.CopyAsync(
            new CopyDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            }
        );

        // Assert.
        for (var i = 0; i < 1001; i++)
        {
            await ExpectFileToExistAsync(
                $"{_destinationDirectory.NormalisedPath}/{i}/.keep".AsBaselineFilesystemPath()
            );
        }
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task AfterDirectoryIsCopied_ItDoesNotResultInActionsOnTheSourceBeingPerformedOnTheDestination(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var originalFirstFilePath =
            $"{_sourceDirectory.NormalisedPath}/a/b.txt".AsBaselineFilesystemPath();
        var originalSecondFilePath =
            $"{_sourceDirectory.NormalisedPath}/a/b/c.txt".AsBaselineFilesystemPath();

        await TestAdapter.CreateFileAndWriteTextAsync(originalFirstFilePath);
        await TestAdapter.CreateFileAndWriteTextAsync(originalSecondFilePath);

        await DirectoryManager.CopyAsync(
            new CopyDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            }
        );

        var afterTheFactThirdFilePath =
            $"{_sourceDirectory.NormalisedPath}/a/b/d.txt".AsBaselineFilesystemPath();

        // Act.
        await TestAdapter.CreateFileAndWriteTextAsync(afterTheFactThirdFilePath);

        // Assert.
        await ExpectFileToExistAsync(originalFirstFilePath);
        await ExpectFileToExistAsync(originalSecondFilePath);
        await ExpectFileToExistAsync(afterTheFactThirdFilePath);
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/a/b.txt".AsBaselineFilesystemPath()
        );
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/a/b/c.txt".AsBaselineFilesystemPath()
        );
        await ExpectFileNotToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/a/b/d.txt".AsBaselineFilesystemPath()
        );
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Copies_A_Directory_With_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        await TestAdapter.CreateFileAndWriteTextAsync(
            $"{_sourceDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath()
        );

        // Act.
        await DirectoryManager.CopyAsync(
            new CopyDirectoryRequest
            {
                SourceDirectoryPath = _sourceDirectory,
                DestinationDirectoryPath = _destinationDirectory
            }
        );

        // Assert.
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/.keep".AsBaselineFilesystemPath()
        );
    }
}
