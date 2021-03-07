using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests.FileManagerTests
{
    public class MoveTests : BaseManagerUsageTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
        {
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = "a".AsBaselineFilesystemPath(), DestinationFilePath = "a".AsBaselineFilesystemPath() },
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
                new MoveFileRequest { SourceFilePath = null, DestinationFilePath = "a".AsBaselineFilesystemPath() }
            );
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Destination_Path_For_The_Request_Was_Null()
        {
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = "a".AsBaselineFilesystemPath(), DestinationFilePath = null }
            );
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Source_Path_Was_Obviously_Intended_As_A_Directory()
        {
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsBaselineFilesystemPath();
            
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = path, DestinationFilePath = "a".AsBaselineFilesystemPath() }
            );
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Destination_Path_Was_Obviously_Intended_As_A_Directory()
        {
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsBaselineFilesystemPath();
            
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = "a".AsBaselineFilesystemPath(), DestinationFilePath = path }
            );
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }
        
        [Fact]
        public async Task It_Invokes_The_Matching_Adapters_Move_File_Method()
        {
            Adapter
                .Setup(x => x.MoveFileAsync(It.IsAny<MoveFileRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FileRepresentation())
                .Verifiable();
            
            await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = "a".AsBaselineFilesystemPath(), DestinationFilePath = "a".AsBaselineFilesystemPath() }
            );
            
            Adapter.VerifyAll();
        }
    }
}
