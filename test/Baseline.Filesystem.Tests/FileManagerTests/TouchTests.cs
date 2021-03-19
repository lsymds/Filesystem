using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests.FileManagerTests
{
    public class TouchTests : BaseManagerUsageTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
        {
            Func<Task> func = async () => await FileManager.TouchAsync(
                new TouchFileRequest { FilePath = "a".AsBaselineFilesystemPath() },
                "foo"
            );
            await func.Should().ThrowAsync<AdapterNotFoundException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_TouchFileRequest_Was_Null()
        {
            Func<Task> func = async () => await FileManager.TouchAsync(null);
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_For_The_TouchFileRequest_Was_Null()
        {
            Func<Task> func = async () => await FileManager.TouchAsync(new TouchFileRequest());
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Path_Was_Obviously_Intended_As_A_Directory()
        {
            var path = "/users/Foo/bar/Destiny/XYZ/BARTINO/".AsBaselineFilesystemPath();
            
            Func<Task> func = async () => await FileManager.TouchAsync(new TouchFileRequest { FilePath = path });
            await func.Should().ThrowExactlyAsync<PathIsADirectoryException>();
        }
        
        [Fact]
        public async Task It_Invokes_The_Matching_Adapters_Touch_File_Method()
        {
            Adapter
                .Setup(x => x.TouchFileAsync(It.IsAny<TouchFileRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FileRepresentation { Path = new PathRepresentation() })
                .Verifiable();
            
            await FileManager.TouchAsync(new TouchFileRequest { FilePath = "a".AsBaselineFilesystemPath() });
            
            Adapter.VerifyAll();
        }
    }
}
