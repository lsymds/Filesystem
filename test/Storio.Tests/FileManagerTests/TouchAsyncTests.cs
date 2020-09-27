using System;
using System.Threading.Tasks;
using FluentAssertions;
using Storio.Requests.Files;
using Xunit;

namespace Storio.Tests.FileManagerTests
{
    public class TouchAsyncTests : BaseFileManagerTests
    {
        [Fact]
        public async Task It_Throws_An_Exception_If_The_Requested_Adapter_Name_Is_Not_Registered()
        {
            Func<Task> func = async () => await FileManager.TouchAsync(new TouchFileRequest(), "foo");
            await func.Should().ThrowAsync<AdapterNotFoundException>();
        }
    }
}
