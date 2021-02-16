using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.DirectoryManagerTests
{
    public class DeleteTests : BaseManagerUsageTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Adapter_Is_Not_Registered()
        {
            Func<Task> func = () => DirectoryManager.DeleteAsync(
                new DeleteDirectoryRequest { DirectoryPath = "i/am/a/directory/".AsBaselineFilesystemPath() },
                "non-existent"
            );
            await func.Should().ThrowExactlyAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_The_Request_Is_Null()
        {
            Func<Task> func = () => DirectoryManager.DeleteAsync(null);
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
        
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Path_In_The_Request_Is_Null()
        {
            Func<Task> func = () => DirectoryManager.DeleteAsync(new DeleteDirectoryRequest());
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_The_Request_Is_Obviously_Not_A_Directory()
        {
            Func<Task> func = () => DirectoryManager.DeleteAsync(new DeleteDirectoryRequest
            {
                DirectoryPath = "i/am/not/a/directory/but-a-path.jpeg".AsBaselineFilesystemPath()
            });
            await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
        }
    }
}
