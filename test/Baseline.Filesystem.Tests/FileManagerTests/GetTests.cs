using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests.FileManagerTests;

public class GetTests : BaseManagerUsageTest
{
    [Fact]
    public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.GetAsync(
                new GetFileRequest { FilePath = "a".AsBaselineFilesystemPath() },
                "foo"
            );

        // Assert.
        await func.Should().ThrowAsync<StoreNotFoundException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () => await FileManager.GetAsync(null);

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Path_For_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () => await FileManager.GetAsync(new GetFileRequest());

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Path_Was_Obviously_Intended_As_A_Directory()
    {
        // Arrange.
        var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsBaselineFilesystemPath();

        // Act.
        Func<Task> func = async () =>
            await FileManager.GetAsync(new GetFileRequest { FilePath = path });

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
    }

    [Fact]
    public async Task It_Invokes_The_Matching_Adapters_Get_File_Method_And_Wraps_The_Response()
    {
        // Arrange.
        Adapter
            .Setup(
                x => x.GetFileAsync(It.IsAny<GetFileRequest>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(
                new GetFileResponse
                {
                    File = new FileRepresentation { Path = new PathRepresentation() }
                }
            )
            .Verifiable();

        // Act.
        await FileManager.GetAsync(
            new GetFileRequest { FilePath = "a".AsBaselineFilesystemPath() }
        );

        // Assert.
        Adapter.VerifyAll();
    }

    [Fact]
    public async Task It_Removes_A_Root_Path_Returned_From_The_Adapter()
    {
        // Arrange.
        Reconfigure(true);

        Adapter
            .Setup(x => x.GetFileAsync(It.IsAny<GetFileRequest>(), CancellationToken.None))
            .ReturnsAsync(
                new GetFileResponse
                {
                    File = new FileRepresentation
                    {
                        Path = $"root/a/b/c.txt".AsBaselineFilesystemPath()
                    }
                }
            );

        // Act.
        var response = await FileManager.GetAsync(
            new GetFileRequest { FilePath = "a/b/c.txt".AsBaselineFilesystemPath(), }
        );

        // Assert.
        response.File.Path.NormalisedPath.Should().NotContain("root");
        response.File.Path.OriginalPath.Should().Be("a/b/c.txt");
    }
}