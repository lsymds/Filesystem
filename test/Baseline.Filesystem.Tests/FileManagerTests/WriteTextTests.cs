using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests.FileManagerTests
{
    public class WriteTextTests : BaseManagerUsageTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
        {
            // Act.
            Func<Task> func = async () => await FileManager.WriteTextAsync(
                new WriteTextToFileRequest { FilePath = "a".AsBaselineFilesystemPath(), TextToWrite = string.Empty },
                "foo"
            );
            
            // Assert.
            await func.Should().ThrowAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Request_Was_Null()
        {
            // Act.
            Func<Task> func = async () => await FileManager.WriteTextAsync(null);
            
            // Assert.
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_For_The_Request_Was_Null()
        {
            // Act.
            Func<Task> func = async () => await FileManager.WriteTextAsync(new WriteTextToFileRequest { TextToWrite = "foo" });
            
            // Assert.
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_TextToWrite_For_The_Request_Was_Null()
        {
            // Act.
            Func<Task> func = async () => await FileManager.WriteTextAsync(
                new WriteTextToFileRequest { FilePath = "a".AsBaselineFilesystemPath() }
            );
            
            // Assert.
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_Was_Obviously_Intended_As_A_Directory()
        {
            // Arrange.
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsBaselineFilesystemPath();
            
            // Act.
            Func<Task> func = async () => await FileManager.WriteTextAsync(
                new WriteTextToFileRequest { FilePath = path, TextToWrite = "abc"}
            );
            
            // Assert.
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }
        
        [Fact]
        public async Task It_Invokes_The_Matching_Adapters_WriteTextToFile_Method()
        {
            // Arrange.
            Adapter
                .Setup(x => x.WriteTextToFileAsync(
                    It.Is<WriteTextToFileRequest>(d => d.TextToWrite == "abc"), 
                    It.IsAny<CancellationToken>())
                )
                .Verifiable();
            
            // Act.
            await FileManager.WriteTextAsync(
                new WriteTextToFileRequest { FilePath = "a".AsBaselineFilesystemPath(), TextToWrite = "abc"}
            );
            
            // Assert.
            Adapter.VerifyAll();
        }
    }
}
