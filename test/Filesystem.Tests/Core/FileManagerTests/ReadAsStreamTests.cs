using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LSymds.Filesystem.Tests.Core.FileManagerTests;

public class ReadAsStreamTests : BaseManagerUsageTest
{
    [Fact]
    public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.ReadAsStreamAsync(
                new ReadFileAsStreamRequest { FilePath = "a".AsBaselineFilesystemPath() },
                "foo"
            );

        // Assert.
        await func.Should().ThrowAsync<StoreNotFoundException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () => await FileManager.ReadAsStreamAsync(null);

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Path_For_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.ReadAsStreamAsync(new ReadFileAsStreamRequest());

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
            await FileManager.ReadAsStreamAsync(
                new ReadFileAsStreamRequest { FilePath = path }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
    }

    [Fact]
    public async Task It_Invokes_The_Matching_Adapters_ReadFileAsString_Method()
    {
        // Arrange.
        Adapter
            .Setup(
                x =>
                    x.ReadFileAsStreamAsync(
                        It.IsAny<ReadFileAsStreamRequest>(),
                        It.IsAny<CancellationToken>()
                    )
            )
            .ReturnsAsync(
                new ReadFileAsStreamResponse
                {
                    FileContents = new MemoryStream(Encoding.UTF8.GetBytes("foo"))
                }
            )
            .Verifiable();

        // Act.
        var response = await FileManager.ReadAsStreamAsync(
            new ReadFileAsStreamRequest { FilePath = "a".AsBaselineFilesystemPath() }
        );

        // Assert.
        (await new StreamReader(response.FileContents).ReadToEndAsync())
            .Should()
            .Be("foo");
        Adapter.VerifyAll();
    }
}
