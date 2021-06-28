using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests.DirectoryManagerTests
{
    public class  CopyTests : BaseManagerUsageTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Adapter_Is_Not_Registered()
        {
            // Act.
            Func<Task> func = () => DirectoryManager.CopyAsync(
                new CopyDirectoryRequest
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
            Func<Task> func = () => DirectoryManager.CopyAsync(null);
            
            // Assert.
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
        
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Source_Path_In_The_Request_Is_Null()
        {
            // Act.
            Func<Task> func = () => DirectoryManager.CopyAsync(new CopyDirectoryRequest());
            
            // Assert.
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
        
        [Fact]
        public async Task It_Throws_An_Exception_When_The_Destination_Path_In_The_Request_Is_Null()
        {
            // Act.
            Func<Task> func = () => DirectoryManager.CopyAsync(new CopyDirectoryRequest
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
            Func<Task> func = () => DirectoryManager.CopyAsync(new CopyDirectoryRequest
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
            Func<Task> func = () => DirectoryManager.CopyAsync(new CopyDirectoryRequest
            {
                SourceDirectoryPath = "/i/am/a/directory/".AsBaselineFilesystemPath(),
                DestinationDirectoryPath = "i/am/not/a/directory/but-a-path.jpeg".AsBaselineFilesystemPath(),
            });
            
            // Assert.
            await func.Should().ThrowExactlyAsync<PathIsNotObviouslyADirectoryException>();
        }

        [Fact]
        public async Task It_Removes_A_Root_Path_Returned_From_The_Adapter()
        {
            // Arrange.
            Reconfigure(true);

            Adapter.Setup(x => x.CopyDirectoryAsync(It.IsAny<CopyDirectoryRequest>(), CancellationToken.None))
                .ReturnsAsync(new CopyDirectoryResponse
                {
                    DestinationDirectory = new DirectoryRepresentation { Path = $"root/a/b/".AsBaselineFilesystemPath() }
                });

            // Act.
            var response = await DirectoryManager.CopyAsync(new CopyDirectoryRequest
            {
                SourceDirectoryPath = "a/a/".AsBaselineFilesystemPath(),
                DestinationDirectoryPath = "a/b/".AsBaselineFilesystemPath()
            });

            // Assert.
            response.DestinationDirectory.Path.NormalisedPath.Should().NotContain("root");
            response.DestinationDirectory.Path.OriginalPath.Should().Be("a/b/");
        }
    }
}
