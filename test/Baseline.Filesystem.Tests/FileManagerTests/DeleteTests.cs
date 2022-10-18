using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests.FileManagerTests;

public class DeleteTests : BaseManagerUsageTest
{
    [Fact]
    public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.DeleteAsync(
                new DeleteFileRequest { FilePath = "a".AsBaselineFilesystemPath() },
                "foo"
            );

        // Assert.
        await func.Should().ThrowAsync<StoreNotFoundException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () => await FileManager.DeleteAsync(null);

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Path_For_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () => await FileManager.DeleteAsync(new DeleteFileRequest());

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
            await FileManager.DeleteAsync(new DeleteFileRequest { FilePath = path });

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
    }

    [Fact]
    public async Task It_Invokes_The_Matching_Adapters_Delete_File_Method()
    {
        // Arrange.
        Adapter
            .Setup(
                x =>
                    x.DeleteFileAsync(
                        It.IsAny<DeleteFileRequest>(),
                        It.IsAny<CancellationToken>()
                    )
            )
            .Verifiable();

        // Act.
        await FileManager.DeleteAsync(
            new DeleteFileRequest { FilePath = "a".AsBaselineFilesystemPath() }
        );

        // Assert.
        Adapter.VerifyAll();
    }
}
