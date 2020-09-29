using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Storio.Tests.FileManagerTests
{
    public class ExistsAsyncTests : BaseFileManagerTests
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
        {
            Func<Task> func = async () => await FileManager.ExistsAsync(
                new FileExistsRequest { FilePath = "a".AsStorioPath() },
                "foo"
            );
            await func.Should().ThrowAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Request_Was_Null()
        {
            Func<Task> func = async () => await FileManager.ExistsAsync(null);
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_For_The_Request_Was_Null()
        {
            Func<Task> func = async () => await FileManager.ExistsAsync(new FileExistsRequest());
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_Was_Obviously_Intended_As_A_Directory()
        {
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsStorioPath();
            
            Func<Task> func = async () => await FileManager.ExistsAsync(new FileExistsRequest { FilePath = path });
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }
        
        [Fact]
        public async Task It_Invokes_The_Matching_Adapters_File_Exists_Method()
        {
            Adapter
                .Setup(x => x.FileExistsAsync(It.IsAny<FileExistsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false)
                .Verifiable();
            
            var response = await FileManager.ExistsAsync(new FileExistsRequest { FilePath = "a".AsStorioPath() });
            response.Should().BeFalse();
            
            Adapter.VerifyAll();
        }
    }
}
