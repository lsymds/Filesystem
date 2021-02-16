using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.DirectoryManagerTests
{
    public class CopyTests : BaseManagerUsageTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Adapter_Is_Not_Registered()
        {
            Func<Task> func = () => DirectoryManager.CopyAsync(
                new CopyDirectoryRequest
                {
                    SourceDirectoryPath = "i/am/a/directory/".AsBaselineFilesystemPath(),
                    DestinationDirectoryPath = "i/am/a/directory/".AsBaselineFilesystemPath()
                },
                "non-existent"
            );
            await func.Should().ThrowExactlyAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_The_Request_Is_Null()
        {
            Func<Task> func = () => DirectoryManager.CopyAsync(null);
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
        
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Source_Path_In_The_Request_Is_Null()
        {
            Func<Task> func = () => DirectoryManager.CopyAsync(new CopyDirectoryRequest());
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
        
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Destination_Path_In_The_Request_Is_Null()
        {
            Func<Task> func = () => DirectoryManager.CopyAsync(new CopyDirectoryRequest
            {
                SourceDirectoryPath = "/foo/".AsBaselineFilesystemPath()
            });
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_The_Source_Path_Is_Obviously_Not_A_Directory()
        {
            Func<Task> func = () => DirectoryManager.CopyAsync(new CopyDirectoryRequest
            {
                SourceDirectoryPath = "i/am/not/a/directory/but-a-path.jpeg".AsBaselineFilesystemPath(),
                DestinationDirectoryPath = "/i/am/a/directory/".AsBaselineFilesystemPath(),
            });
            await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
        }
        
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Destination_Path_Is_Obviously_Not_A_Directory()
        {
            Func<Task> func = () => DirectoryManager.CopyAsync(new CopyDirectoryRequest
            {
                SourceDirectoryPath = "/i/am/a/directory/".AsBaselineFilesystemPath(),
                DestinationDirectoryPath = "i/am/not/a/directory/but-a-path.jpeg".AsBaselineFilesystemPath(),
            });
            await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
        }
    }
}
