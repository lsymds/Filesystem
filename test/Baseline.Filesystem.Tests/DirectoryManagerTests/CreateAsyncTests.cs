using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.DirectoryManagerTests
{
    public class CreateAsyncManagerUsageTests : BaseManagerUsageTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Adapter_Is_Not_Registered()
        {
            Func<Task> func = () => DirectoryManager.CreateAsync(
                new CreateDirectoryRequest { DirectoryPath = "i/am/a/directory/".AsBaselineFilesystemPath() },
                "non-existent"
            );
            await func.Should().ThrowExactlyAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_The_Request_Is_Null()
        {
            Func<Task> func = () => DirectoryManager.CreateAsync(null);
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
        
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Path_In_The_Request_Is_Null()
        {
            Func<Task> func = () => DirectoryManager.CreateAsync(new CreateDirectoryRequest());
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_When_The_Request_Is_Obviously_Not_A_Directory()
        {
            Func<Task> func = () => DirectoryManager.CreateAsync(new CreateDirectoryRequest
            {
                DirectoryPath = "i/am/not/a/directory/but-a-path.jpeg".AsBaselineFilesystemPath()
            });
            await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
        }
        
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Request_Is_Not_Clearly_A_Directory()
        {
            Func<Task> func = () => DirectoryManager.CreateAsync(new CreateDirectoryRequest
            {
                DirectoryPath = "i/am/not/a/directory/or/am/i".AsBaselineFilesystemPath()
            });
            await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
        }
    }
}
