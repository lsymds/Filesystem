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
            // Act.
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = "a".AsBaselineFilesystemPath(), DestinationFilePath = "a".AsBaselineFilesystemPath() },
                "foo"
            );
            
            // Assert.
            await func.Should().ThrowAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Request_Was_Null()
        {
            // Act.
            Func<Task> func = async () => await FileManager.MoveAsync(null);
            
            // Assert.
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Source_Path_For_The_Request_Was_Null()
        {
            // Act.
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = null, DestinationFilePath = "a".AsBaselineFilesystemPath() }
            );
            
            // Assert.
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Destination_Path_For_The_Request_Was_Null()
        {
            // Act.
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = "a".AsBaselineFilesystemPath(), DestinationFilePath = null }
            );
            
            // Assert.
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Source_Path_Was_Obviously_Intended_As_A_Directory()
        {
            // Arrange.
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsBaselineFilesystemPath();
            
            // Act.
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = path, DestinationFilePath = "a".AsBaselineFilesystemPath() }
            );
            
            // Assert.
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Destination_Path_Was_Obviously_Intended_As_A_Directory()
        {
            // Arrange.
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsBaselineFilesystemPath();
            
            // Act.
            Func<Task> func = async () => await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = "a".AsBaselineFilesystemPath(), DestinationFilePath = path }
            );
            
            // Assert.
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }
        
        [Fact]
        public async Task It_Invokes_The_Matching_Adapters_Move_File_Method()
        {
            // Arrange.
            Adapter
                .Setup(x => x.MoveFileAsync(It.IsAny<MoveFileRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MoveFileResponse { DestinationFile = new FileRepresentation() })
                .Verifiable();
            
            // Act.
            await FileManager.MoveAsync(
                new MoveFileRequest { SourceFilePath = "a".AsBaselineFilesystemPath(), DestinationFilePath = "a".AsBaselineFilesystemPath() }
            );
            
            // Assert.
            Adapter.VerifyAll();
        }
    }
}
