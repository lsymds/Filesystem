using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LSymds.Filesystem.Tests.Core.FileManagerTests;

public class ExistsTests : BaseManagerUsageTest
{
    [Fact]
    public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.ExistsAsync(
                new FileExistsRequest { FilePath = "a".AsFilesystemPath() },
                "foo"
            );

        // Assert.
        await func.Should().ThrowAsync<StoreNotFoundException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () => await FileManager.ExistsAsync(null);

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Path_For_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () => await FileManager.ExistsAsync(new FileExistsRequest());

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Path_Was_Obviously_Intended_As_A_Directory()
    {
        // Arrange.
        var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsFilesystemPath();

        // Act.
        Func<Task> func = async () =>
            await FileManager.ExistsAsync(new FileExistsRequest { FilePath = path });

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
    }

    [Fact]
    public async Task It_Invokes_The_Matching_Adapters_File_Exists_Method()
    {
        // Arrange.
        Adapter
            .Setup(
                x =>
                    x.FileExistsAsync(
                        It.IsAny<FileExistsRequest>(),
                        It.IsAny<CancellationToken>()
                    )
            )
            .ReturnsAsync(new FileExistsResponse { FileExists = false })
            .Verifiable();

        // Act.
        var response = await FileManager.ExistsAsync(
            new FileExistsRequest { FilePath = "a".AsFilesystemPath() }
        );

        // Assert.
        response.FileExists.Should().BeFalse();
        Adapter.VerifyAll();
    }
}
