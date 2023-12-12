using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LSymds.Filesystem.Tests.Core;

public class GeneralTests : BaseManagerUsageTest
{
    [Fact]
    public async Task When_A_Method_Wraps_Exceptions_They_Wrap_Successfully()
    {
        // Arrange.
        Adapter
            .Setup(x => x.GetFileAsync(It.IsAny<GetFileRequest>(), CancellationToken.None))
            .ThrowsAsync(new ExternalException());

        // Act.
        Func<Task> act = async () =>
            await FileManager.GetAsync(
                new GetFileRequest { FilePath = "abc".AsFilesystemPath(), },
                "default",
                CancellationToken.None
            );

        // Assert.
        await act.Should()
            .ThrowExactlyAsync<StoreAdapterOperationException>()
            .WithMessage(
                "Unhandled exception thrown from the adapter for store 'default', potentially whilst communicating with "
                + "its API. See the inner exception for details."
            );
    }

    [Fact]
    public async Task It_Clones_The_Request_Passed_In_And_Does_Not_Modify_The_Original_Request()
    {
        // Arrange.
        Reconfigure(true);

        var filePath = "abc".AsFilesystemPath();

        var request = new FileExistsRequest { FilePath = filePath };

        Adapter
            .Setup(
                x =>
                    x.FileExistsAsync(
                        It.Is<FileExistsRequest>(
                            p => p.FilePath.OriginalPath == $"root/{filePath.OriginalPath}"
                        ),
                        It.IsAny<CancellationToken>()
                    )
            )
            .ReturnsAsync(new FileExistsResponse { FileExists = true })
            .Verifiable();

        // Act.
        await FileManager.ExistsAsync(request);

        // Assert.
        Adapter.Verify();
        request.FilePath.Should().Be(filePath);
    }
}
