using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.DirectoryManagerTests
{
    public class MoveTests : BaseManagerUsageTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Adapter_Is_Not_Registered()
        {
            // Act.
            Func<Task> func = () => DirectoryManager.MoveAsync(
                new MoveDirectoryRequest
                {
                    SourceDirectoryPath = "i/am/a/directory/".AsBaselineFilesystemPath(),
                    DestinationDirectoryPath = "i/am/a/directory/".AsBaselineFilesystemPath()
                },
                "non-existent"
            );
            
            // Assert.
            await func.Should().ThrowExactlyAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_The_Request_Is_Null()
        {
            // Act.
            Func<Task> func = () => DirectoryManager.MoveAsync(null);
            
            // Assert.
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
        
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Source_Path_In_The_Request_Is_Null()
        {
            // Act.
            Func<Task> func = () => DirectoryManager.MoveAsync(new MoveDirectoryRequest());
            
            // Assert.
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
        
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Destination_Path_In_The_Request_Is_Null()
        {
            // Act.
            Func<Task> func = () => DirectoryManager.MoveAsync(new MoveDirectoryRequest
            {
                SourceDirectoryPath = "/foo/".AsBaselineFilesystemPath()
            });
            
            // Assert.
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_The_Source_Path_Is_Obviously_Not_A_Directory()
        {
            // Act.
            Func<Task> func = () => DirectoryManager.MoveAsync(new MoveDirectoryRequest
            {
                SourceDirectoryPath = "i/am/not/a/directory/but-a-path.jpeg".AsBaselineFilesystemPath(),
                DestinationDirectoryPath = "/i/am/a/directory/".AsBaselineFilesystemPath(),
            });
            
            // Assert.
            await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
        }
        
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Destination_Path_Is_Obviously_Not_A_Directory()
        {
            // Act.
            Func<Task> func = () => DirectoryManager.MoveAsync(new MoveDirectoryRequest
            {
                SourceDirectoryPath = "/i/am/a/directory/".AsBaselineFilesystemPath(),
                DestinationDirectoryPath = "i/am/not/a/directory/but-a-path.jpeg".AsBaselineFilesystemPath(),
            });
            
            // Assert.
            await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
        }
    }
}
