using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace LSymds.Filesystem.Tests.Core.FileManagerTests;

public class GetPublicUrlTests : BaseManagerUsageTest
{
    [Fact]
    public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.GetPublicUrlAsync(
                new GetFilePublicUrlRequest { FilePath = "a".AsFilesystemPath() },
                "foo"
            );

        // Assert.
        await func.Should().ThrowAsync<StoreNotFoundException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () => await FileManager.GetPublicUrlAsync(null);

        // Assert.
        await func.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Path_For_The_Request_Was_Null()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.GetPublicUrlAsync(new GetFilePublicUrlRequest());

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
            await FileManager.GetPublicUrlAsync(
                new GetFilePublicUrlRequest { FilePath = path }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Expiry_Date_Was_Not_Far_Enough_Away()
    {
        // Arrange.
        var path = "/users/Foo/bar/Destiny/XYZ/BARTINO.txt".AsFilesystemPath();

        // Act.
        Func<Task> func = async () =>
            await FileManager.GetPublicUrlAsync(
                new GetFilePublicUrlRequest { FilePath = path, Expiry = DateTime.Now }
            );

        // Assert.
        await func.Should()
            .ThrowExactlyAsync<ArgumentException>()
            .WithMessage(
                "Expiry cannot be less than 10 seconds away from the current time. (Parameter 'Expiry')"
            );
    }

    [Fact]
    public async Task It_Invokes_The_Matching_Adapters_Get_File_Method_And_Wraps_The_Response()
    {
        // Arrange.
        Adapter
            .Setup(
                x =>
                    x.GetFilePublicUrlAsync(
                        It.IsAny<GetFilePublicUrlRequest>(),
                        It.IsAny<CancellationToken>()
                    )
            )
            .ReturnsAsync(new GetFilePublicUrlResponse { Url = "https://www.google.com" })
            .Verifiable();

        // Act.
        var response = await FileManager.GetPublicUrlAsync(
            new GetFilePublicUrlRequest { FilePath = "a".AsFilesystemPath() }
        );

        // Assert.
        response.Url.Should().Be("https://www.google.com");
        Adapter.VerifyAll();
    }
}
