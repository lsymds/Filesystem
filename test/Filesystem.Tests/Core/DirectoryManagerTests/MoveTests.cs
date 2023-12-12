using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LSymds.Filesystem.Tests.Core.DirectoryManagerTests;

public class MoveTests : BaseManagerUsageTest
{
    [Fact]
    public async Task It_Throws_An_Exception_When_The_Store_Is_Not_Registered()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.MoveAsync(
                new MoveDirectoryRequest
                {
                    SourceDirectoryPath = "i/am/a/directory/".AsFilesystemPath(),
                    DestinationDirectoryPath = "i/am/a/directory/".AsFilesystemPath()
                },
                "non-existent"
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<StoreNotFoundException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Request_Is_Null()
    {
        // Act.
        Func<Task> func = () => DirectoryManager.MoveAsync(null);

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Source_Path_In_The_Request_Is_Null()
    {
        // Act.
        Func<Task> func = () => DirectoryManager.MoveAsync(new MoveDirectoryRequest());

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Destination_Path_In_The_Request_Is_Null()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.MoveAsync(
                new MoveDirectoryRequest
                {
                    SourceDirectoryPath = "/foo/".AsFilesystemPath()
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Source_Path_Is_Obviously_Not_A_Directory()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.MoveAsync(
                new MoveDirectoryRequest
                {
                    SourceDirectoryPath =
                        "i/am/not/a/directory/but-a-path.jpeg".AsFilesystemPath(),
                    DestinationDirectoryPath = "/i/am/a/directory/".AsFilesystemPath(),
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Destination_Path_Is_Obviously_Not_A_Directory()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.MoveAsync(
                new MoveDirectoryRequest
                {
                    SourceDirectoryPath = "/i/am/a/directory/".AsFilesystemPath(),
                    DestinationDirectoryPath =
                        "i/am/not/a/directory/but-a-path.jpeg".AsFilesystemPath(),
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
    }

    [Fact]
    public async Task It_Removes_A_Root_Path_Returned_From_The_Store()
    {
        // Arrange.
        Reconfigure(true);

        Adapter
            .Setup(
                x =>
                    x.MoveDirectoryAsync(
                        It.IsAny<MoveDirectoryRequest>(),
                        CancellationToken.None
                    )
            )
            .ReturnsAsync(
                new MoveDirectoryResponse
                {
                    DestinationDirectory = new DirectoryRepresentation
                    {
                        Path = $"root/a/b/".AsFilesystemPath()
                    }
                }
            );

        // Act.
        var response = await DirectoryManager.MoveAsync(
            new MoveDirectoryRequest
            {
                SourceDirectoryPath = "a/a/".AsFilesystemPath(),
                DestinationDirectoryPath = "a/b/".AsFilesystemPath()
            }
        );

        // Assert.
        response.DestinationDirectory.Path.NormalisedPath.Should().NotContain("root");
        response.DestinationDirectory.Path.OriginalPath.Should().Be("a/b/");
    }
}
