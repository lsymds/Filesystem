using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests.FileManagerTests
{
    public class GetPublicUrlAsyncTests : BaseManagerUsageTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
        {
            Func<Task> func = async () => await FileManager.GetPublicUrlAsync(
                new GetFilePublicUrlRequest { FilePath = "a".AsBaselineFilesystemPath() },
                "foo"
            );
            await func.Should().ThrowAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Request_Was_Null()
        {
            Func<Task> func = async () => await FileManager.GetPublicUrlAsync(null);
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_For_The_Request_Was_Null()
        {
            Func<Task> func = async () => await FileManager.GetPublicUrlAsync(new GetFilePublicUrlRequest());
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_Was_Obviously_Intended_As_A_Directory()
        {
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsBaselineFilesystemPath();
            
            Func<Task> func = async () => await FileManager.GetPublicUrlAsync(new GetFilePublicUrlRequest
            {
                FilePath = path
            });
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Expiry_Date_Was_Not_Far_Enough_Away()
        {
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO.txt".AsBaselineFilesystemPath();
            
            Func<Task> func = async () => await FileManager.GetPublicUrlAsync(new GetFilePublicUrlRequest
            {
                FilePath = path,
                Expiry = DateTime.Now
            });
            await func
                .Should()
                .ThrowExactlyAsync<ArgumentException>()
                .WithMessage("Expiry cannot be less than 10 seconds away from the current time. (Parameter 'Expiry')");
        }
        
        [Fact]
        public async Task It_Invokes_The_Matching_Adapters_Get_File_Method_And_Wraps_The_Response()
        {
            Adapter
                .Setup(x => x.GetFilePublicUrlAsync(It.IsAny<GetFilePublicUrlRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetFilePublicUrlResponse { Url = "https://www.google.com" })
                .Verifiable();
            
            var response = await FileManager.GetPublicUrlAsync(new GetFilePublicUrlRequest
            {
                FilePath = "a".AsBaselineFilesystemPath()
            });
            response.Url.Should().Be("https://www.google.com");
            
            Adapter.VerifyAll();
        }
    }
}
