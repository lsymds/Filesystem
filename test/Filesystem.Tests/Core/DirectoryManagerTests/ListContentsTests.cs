using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LSymds.Filesystem.Tests.Core.DirectoryManagerTests;

public class ListContentsTests : BaseManagerUsageTest
{
    [Fact]
    public async Task It_Throws_An_Exception_When_The_Adapter_Is_Not_Registered()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.ListContentsAsync(
                new ListDirectoryContentsRequest
                {
                    DirectoryPath = "i/am/a/directory/".AsFilesystemPath()
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
        Func<Task> func = () => DirectoryManager.ListContentsAsync(null);

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Path_In_The_Request_Is_Null()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.ListContentsAsync(new ListDirectoryContentsRequest());

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Request_Is_Obviously_Not_A_Directory()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.ListContentsAsync(
                new ListDirectoryContentsRequest
                {
                    DirectoryPath =
                        "i/am/not/a/directory/but-a-path.jpeg".AsFilesystemPath()
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
    }

    [Fact]
    public async Task It_Removes_A_Root_Path_Returned_From_The_Adapter()
    {
        // Arrange.
        Reconfigure(true);

        Adapter
            .Setup(
                x =>
                    x.ListDirectoryContentsAsync(
                        It.IsAny<ListDirectoryContentsRequest>(),
                        CancellationToken.None
                    )
            )
            .ReturnsAsync(
                new ListDirectoryContentsResponse
                {
                    Contents = new List<PathRepresentation>
                    {
                        "root/a/b/".AsFilesystemPath(),
                        "root/a/b/c.txt".AsFilesystemPath()
                    }
                }
            );

        // Act.
        var response = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest { DirectoryPath = "a/".AsFilesystemPath() }
        );

        // Assert.
        response.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b");
        response.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/c.txt");
        response.Contents.Should().NotContain(x => x.NormalisedPath.Contains("root"));
    }
}