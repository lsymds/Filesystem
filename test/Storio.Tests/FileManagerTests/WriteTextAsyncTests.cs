using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Storio.Tests.FileManagerTests
{
    public class WriteTextAsyncTests : BaseFileManagerTests
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
        {
            Func<Task> func = async () => await FileManager.WriteTextAsync(
                new WriteTextToFileRequest { FilePath = "a".AsStorioPath(), TextToWrite = string.Empty },
                "foo"
            );
            await func.Should().ThrowAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_WriteTextToFileRequest_Was_Null()
        {
            Func<Task> func = async () => await FileManager.WriteTextAsync(null);
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_For_The_WriteTextToFileRequest_Was_Null()
        {
            Func<Task> func = async () => await FileManager.WriteTextAsync(new WriteTextToFileRequest());
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_TextToWrite_For_The_WriteTextToFileRequest_Was_Null()
        {
            Func<Task> func = async () => await FileManager.WriteTextAsync(
                new WriteTextToFileRequest { FilePath = "a".AsStorioPath() }
            );
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_Was_Obviously_Intended_As_A_Directory()
        {
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsStorioPath();
            
            Func<Task> func = async () => await FileManager.WriteTextAsync(
                new WriteTextToFileRequest { FilePath = path, TextToWrite = "abc"}
            );
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }
        
        [Fact]
        public async Task It_Invokes_The_Matching_Adapters_WriteTextToFile_Method()
        {
            Adapter
                .Setup(x => x.WriteTextToFileAsync(
                    It.Is<WriteTextToFileRequest>(d => d.TextToWrite == "abc"), 
                    It.IsAny<CancellationToken>())
                )
                .Verifiable();
            
            await FileManager.WriteTextAsync(
                new WriteTextToFileRequest { FilePath = "a".AsStorioPath(), TextToWrite = "abc"}
            );
            
            Adapter.VerifyAll();
        }
    }
}
