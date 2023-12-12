using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LSymds.Filesystem.Tests.Core.FileManagerTests;

public class WriteStreamTests : BaseManagerUsageTest
{
    [Fact]
    public async Task It_Throws_An_Exception_If_The_Requested_Store_Name_Is_Not_Registered()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.WriteStreamAsync(
                new WriteStreamToFileRequest
                {
                    FilePath = "a".AsFilesystemPath(),
                    Stream = new MemoryStream(Encoding.UTF8.GetBytes("hello"))
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
        Func<Task> func = async () => await FileManager.WriteStreamAsync(null);

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Path_For_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.WriteStreamAsync(
                new WriteStreamToFileRequest
                {
                    Stream = new MemoryStream(Encoding.UTF8.GetBytes("hello"))
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Stream_For_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.WriteStreamAsync(
                new WriteStreamToFileRequest { FilePath = "a".AsFilesystemPath() }
            );

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
            await FileManager.WriteStreamAsync(
                new WriteStreamToFileRequest
                {
                    FilePath = path,
                    Stream = new MemoryStream(Encoding.UTF8.GetBytes("hello"))
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
    }

    [Fact]
    public async Task It_Invokes_The_Matching_Adapters_WriteStreamToFile_Method()
    {
        // Arrange.
        Adapter
            .Setup(
                x =>
                    x.WriteStreamToFileAsync(
                        It.IsAny<WriteStreamToFileRequest>(),
                        It.IsAny<CancellationToken>()
                    )
            )
            .Verifiable();

        // Act.
        await FileManager.WriteStreamAsync(
            new WriteStreamToFileRequest
            {
                FilePath = "a".AsFilesystemPath(),
                Stream = new MemoryStream(Encoding.UTF8.GetBytes("hello"))
            }
        );

        // Assert.
        Adapter.VerifyAll();
    }
}
