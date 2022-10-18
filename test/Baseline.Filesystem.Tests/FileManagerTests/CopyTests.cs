using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests.FileManagerTests;

public class CopyTests : BaseManagerUsageTest
{
    [Fact]
    public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = "a".AsBaselineFilesystemPath(),
                    DestinationFilePath = "a".AsBaselineFilesystemPath()
                },
                "foo"
            );

        // Assert.
        await func.Should().ThrowAsync<StoreNotFoundException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () => await FileManager.CopyAsync(null);

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Source_Path_For_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = null,
                    DestinationFilePath = "a".AsBaselineFilesystemPath()
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Destination_Path_For_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = "a".AsBaselineFilesystemPath(),
                    DestinationFilePath = null
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Source_Path_Was_Obviously_Intended_As_A_Directory()
    {
        // Arrange.
        var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsBaselineFilesystemPath();

        // Act.
        Func<Task> func = async () =>
            await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = path,
                    DestinationFilePath = "a".AsBaselineFilesystemPath()
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Destination_Path_Was_Obviously_Intended_As_A_Directory()
    {
        // Arrange.
        var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsBaselineFilesystemPath();

        // Act.
        Func<Task> func = async () =>
            await FileManager.CopyAsync(
                new CopyFileRequest
                {
                    SourceFilePath = "a".AsBaselineFilesystemPath(),
                    DestinationFilePath = path
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
    }

    [Fact]
    public async Task It_Invokes_The_Matching_Adapters_Copy_File_Method()
    {
        // Arrange.
        Adapter
            .Setup(
                x => x.CopyFileAsync(It.IsAny<CopyFileRequest>(), It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(new CopyFileResponse())
            .Verifiable();

        // Act.
        await FileManager.CopyAsync(
            new CopyFileRequest
            {
                SourceFilePath = "a".AsBaselineFilesystemPath(),
                DestinationFilePath = "a".AsBaselineFilesystemPath()
            }
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
            .Setup(x => x.CopyFileAsync(It.IsAny<CopyFileRequest>(), CancellationToken.None))
            .ReturnsAsync(
                new CopyFileResponse
                {
                    DestinationFile = new FileRepresentation
                    {
                        Path = $"root/a/b/c.txt".AsBaselineFilesystemPath()
                    }
                }
            );

        // Act.
        var response = await FileManager.CopyAsync(
            new CopyFileRequest
            {
                SourceFilePath = "a/b/a.txt".AsBaselineFilesystemPath(),
                DestinationFilePath = "a/b/c.txt".AsBaselineFilesystemPath()
            }
        );

        // Assert.
        response.DestinationFile.Path.NormalisedPath.Should().NotContain("root");
        response.DestinationFile.Path.OriginalPath.Should().Be("a/b/c.txt");
    }
}
