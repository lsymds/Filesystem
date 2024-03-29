using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Core.DirectoryManagerTests;

public class IterateContentsTests : BaseManagerUsageTest
{
    [Fact]
    public async Task It_Throws_An_Exception_When_The_Adapter_Is_Not_Registered()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.IterateContentsAsync(
                new IterateDirectoryContentsRequest
                {
                    DirectoryPath = "i/am/a/directory/".AsFilesystemPath(),
                    Action = (_) => null
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
        Func<Task> func = () => DirectoryManager.IterateContentsAsync(null);

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Path_In_The_Request_Is_Null()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.IterateContentsAsync(
                new IterateDirectoryContentsRequest { Action = (_) => null }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Request_Is_Obviously_Not_A_Directory()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.IterateContentsAsync(
                new IterateDirectoryContentsRequest()
                {
                    DirectoryPath =
                        "i/am/not/a/directory/but-a-path.jpeg".AsFilesystemPath(),
                    Action = (_) => null
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Requests_Action_Is_Not_Set()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.IterateContentsAsync(
                new IterateDirectoryContentsRequest()
                {
                    DirectoryPath = "i/am/a/directory/".AsFilesystemPath()
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }
}
