using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests.FileManagerTests;

public class TouchTests : BaseManagerUsageTest
{
    [Fact]
    public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.TouchAsync(
                new TouchFileRequest { FilePath = "a".AsBaselineFilesystemPath() },
                "foo"
            );

        // Assert.
        await func.Should().ThrowAsync<StoreNotFoundException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_TouchFileRequest_Was_Null()
    {
        // Act.
        Func<Task> func = async () => await FileManager.TouchAsync(null);

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Path_For_The_TouchFileRequest_Was_Null()
    {
        // Act.
        Func<Task> func = async () => await FileManager.TouchAsync(new TouchFileRequest());

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
            await FileManager.TouchAsync(new TouchFileRequest { FilePath = path });

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
    }

    [Fact]
    public async Task It_Invokes_The_Matching_Adapters_Touch_File_Method()
    {
        // Arrange.
        Adapter
            .Setup(
                x =>
                    x.TouchFileAsync(
                        It.IsAny<TouchFileRequest>(),
                        It.IsAny<CancellationToken>()
                    )
            )
            .ReturnsAsync(
                new TouchFileResponse
                {
                    File = new FileRepresentation { Path = new PathRepresentation() }
                }
            )
            .Verifiable();

        // Act.
        await FileManager.TouchAsync(
            new TouchFileRequest { FilePath = "a".AsBaselineFilesystemPath() }
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
            .Setup(x => x.TouchFileAsync(It.IsAny<TouchFileRequest>(), CancellationToken.None))
            .ReturnsAsync(
                new TouchFileResponse
                {
                    File = new FileRepresentation
                    {
                        Path = $"root/a/b/c.txt".AsBaselineFilesystemPath()
                    }
                }
            );

        // Act.
        var response = await FileManager.TouchAsync(
            new TouchFileRequest { FilePath = "a/b/c.txt".AsBaselineFilesystemPath(), }
        );

        // Assert.
        response.File.Path.NormalisedPath.Should().NotContain("root");
        response.File.Path.OriginalPath.Should().Be("a/b/c.txt");
    }
}
