using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Storio.Tests.FileManagerTests
{
    public class MoveAsyncTests : BaseFileManagerTests
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
        {
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = "a".AsStorioPath(), DestinationFilePath = "a".AsStorioPath() },
                "foo"
            );
            await func.Should().ThrowAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Request_Was_Null()
        {
            Func<Task> func = async () => await FileManager.MoveAsync(null);
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Source_Path_For_The_Request_Was_Null()
        {
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = null, DestinationFilePath = "a".AsStorioPath() }
            );
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Destination_Path_For_The_Request_Was_Null()
        {
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = "a".AsStorioPath(), DestinationFilePath = null }
            );
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Source_Path_Was_Obviously_Intended_As_A_Directory()
        {
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsStorioPath();
            
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = path, DestinationFilePath = "a".AsStorioPath() }
            );
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Destination_Path_Was_Obviously_Intended_As_A_Directory()
        {
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsStorioPath();
            
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = "a".AsStorioPath(), DestinationFilePath = path }
            );
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }
        
        [Fact]
        public async Task It_Invokes_The_Matching_Adapters_Move_File_Method_And_Wraps_The_Response()
        {
            Adapter
                .Setup(x => x.MoveFileAsync(It.IsAny<MoveFileRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FileRepresentation())
                .Verifiable();
            
            var response = await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = "a".AsStorioPath(), DestinationFilePath = "a".AsStorioPath() }
            );
            response.AdapterName.Should().Be("default");
            
            Adapter.VerifyAll();
        }
    }
}
