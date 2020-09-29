using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Storio.Tests.FileManagerTests
{
    public class GetAsyncTests : BaseFileManagerTests
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
        {
            Func<Task> func = async () => await FileManager.GetAsync(
                new GetFileRequest { FilePath = "a".AsStorioPath() },
                "foo"
            );
            await func.Should().ThrowAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_GetFileRequest_Was_Null()
        {
            Func<Task> func = async () => await FileManager.GetAsync(null);
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_For_The_GetFileRequest_Was_Null()
        {
            Func<Task> func = async () => await FileManager.GetAsync(new GetFileRequest());
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_Was_Obviously_Intended_As_A_Directory()
        {
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsStorioPath();
            
            Func<Task> func = async () => await FileManager.GetAsync(new GetFileRequest { FilePath = path });
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }
        
        [Fact]
        public async Task It_Invokes_The_Matching_Adapters_Get_File_Method_And_Wraps_The_Response()
        {
            Adapter
                .Setup(x => x.GetFileAsync(It.IsAny<GetFileRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FileRepresentation { Path = new PathRepresentation() })
                .Verifiable();
            
            var response = await FileManager.GetAsync(new GetFileRequest { FilePath = "a".AsStorioPath() });
            response.AdapterName.Should().Be("default");
            
            Adapter.VerifyAll();
        }
    }
}
