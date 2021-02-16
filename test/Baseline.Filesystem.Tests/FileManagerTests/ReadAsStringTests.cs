using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests.FileManagerTests
{
    public class ReadAsStringTests : BaseManagerUsageTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
        {
            Func<Task> func = async () => await FileManager.ReadAsStringAsync(
                new ReadFileAsStringRequest { FilePath = "a".AsBaselineFilesystemPath() },
                "foo"
            );
            await func.Should().ThrowAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Request_Was_Null()
        {
            Func<Task> func = async () => await FileManager.ReadAsStringAsync(null);
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_For_The_Request_Was_Null()
        {
            Func<Task> func = async () => await FileManager.ReadAsStringAsync(new ReadFileAsStringRequest());
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_Was_Obviously_Intended_As_A_Directory()
        {
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsBaselineFilesystemPath();

            Func<Task> func = async () =>
                await FileManager.ReadAsStringAsync(new ReadFileAsStringRequest { FilePath = path }
            );
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }
        
        [Fact]
        public async Task It_Invokes_The_Matching_Adapters_ReadFileAsString_Method()
        {
            Adapter
                .Setup(x => x.ReadFileAsStringAsync(It.IsAny<ReadFileAsStringRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("foo")
                .Verifiable();
            
            var response = await FileManager.ReadAsStringAsync(
                new ReadFileAsStringRequest { FilePath = "a".AsBaselineFilesystemPath() }
            );
            response.Should().Be("foo");
            
            Adapter.VerifyAll();
        }
    }
}
