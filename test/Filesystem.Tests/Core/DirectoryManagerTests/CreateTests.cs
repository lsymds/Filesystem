using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LSymds.Filesystem.Tests.Core.DirectoryManagerTests;

public class CreateTests : BaseManagerUsageTest
{
    [Fact]
    public async Task It_Throws_An_Exception_When_The_Store_Is_Not_Registered()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.CreateAsync(
                new CreateDirectoryRequest
                {
                    DirectoryPath = "i/am/a/directory/".AsBaselineFilesystemPath()
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
        Func<Task> func = () => DirectoryManager.CreateAsync(null);

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Path_In_The_Request_Is_Null()
    {
        // Act.
        Func<Task> func = () => DirectoryManager.CreateAsync(new CreateDirectoryRequest());

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Request_Is_Obviously_Not_A_Directory()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.CreateAsync(
                new CreateDirectoryRequest
                {
                    DirectoryPath =
                        "i/am/not/a/directory/but-a-path.jpeg".AsBaselineFilesystemPath()
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_When_The_Request_Is_Not_Clearly_A_Directory()
    {
        // Act.
        Func<Task> func = () =>
            DirectoryManager.CreateAsync(
                new CreateDirectoryRequest
                {
                    DirectoryPath = "i/am/not/a/directory/or/am/i".AsBaselineFilesystemPath()
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
                    x.CreateDirectoryAsync(
                        It.IsAny<CreateDirectoryRequest>(),
                        CancellationToken.None
                    )
            )
            .ReturnsAsync(
                new CreateDirectoryResponse
                {
                    Directory = new DirectoryRepresentation
                    {
                        Path = $"root/a/b/".AsBaselineFilesystemPath()
                    }
                }
            );

        // Act.
        var response = await DirectoryManager.CreateAsync(
            new CreateDirectoryRequest { DirectoryPath = "a/b/".AsBaselineFilesystemPath() }
        );

        // Assert.
        response.Directory.Path.NormalisedPath.Should().NotContain("root");
        response.Directory.Path.OriginalPath.Should().Be("a/b/");
    }
}
