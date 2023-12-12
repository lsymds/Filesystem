using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Adapters.Directories;

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
            $"{_sourceDirectory.NormalisedPath}/.keep".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            $"{_destinationDirectory.NormalisedPath}/.keep".AsFilesystemPath()
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

            await ExpectFileToExistAsync(newDirectoryPath.AsFilesystemPath());
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

        var originalDirectory = "cheese/".AsFilesystemPath();
        var originalFile =
            "cheese/cheese/more-cheese/my-favourite-cheesestring.jpeg".AsFilesystemPath();

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
            $"{_destinationDirectory.NormalisedPath}/cheese/more-cheese/my-favourite-cheesestring.jpeg".AsFilesystemPath()
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
            "a/more/complex/directory/structure/.keep".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/more/complex/directory/structure/with/a/nested/file.jpeg".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/more/complex/directory/structure/with/an/even/more/complex/file/structure.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/more/complex/directory/structure/with/an/even/more/complex/file/structure.config".AsFilesystemPath()
        );

        // Act.
        await DirectoryManager.CopyAsync(
            new CopyDirectoryRequest()
            {
                SourceDirectoryPath =
                    "a/more/complex/directory/structure/".AsFilesystemPath(),
                DestinationDirectoryPath = _destinationDirectory
            }
        );

        // Assert.
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/.keep".AsFilesystemPath()
        );
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/with/a/nested/file.jpeg".AsFilesystemPath()
        );
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/with/an/even/more/complex/file/structure.txt".AsFilesystemPath()
        );
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/with/an/even/more/complex/file/structure.config".AsFilesystemPath()
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
                $"{_sourceDirectory.NormalisedPath}/{i}/.keep".AsFilesystemPath()
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
                $"{_destinationDirectory.NormalisedPath}/{i}/.keep".AsFilesystemPath()
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
            $"{_sourceDirectory.NormalisedPath}/a/b.txt".AsFilesystemPath();
        var originalSecondFilePath =
            $"{_sourceDirectory.NormalisedPath}/a/b/c.txt".AsFilesystemPath();

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
            $"{_sourceDirectory.NormalisedPath}/a/b/d.txt".AsFilesystemPath();

        // Act.
        await TestAdapter.CreateFileAndWriteTextAsync(afterTheFactThirdFilePath);

        // Assert.
        await ExpectFileToExistAsync(originalFirstFilePath);
        await ExpectFileToExistAsync(originalSecondFilePath);
        await ExpectFileToExistAsync(afterTheFactThirdFilePath);
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/a/b.txt".AsFilesystemPath()
        );
        await ExpectFileToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/a/b/c.txt".AsFilesystemPath()
        );
        await ExpectFileNotToExistAsync(
            $"{_destinationDirectory.NormalisedPath}/a/b/d.txt".AsFilesystemPath()
        );
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Copies_A_Directory_With_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        await TestAdapter.CreateFileAndWriteTextAsync(
            $"{_sourceDirectory.NormalisedPath}/.keep".AsFilesystemPath()
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
            $"{_destinationDirectory.NormalisedPath}/.keep".AsFilesystemPath()
        );
    }
}
