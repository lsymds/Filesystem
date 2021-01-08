using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests
{
    public class GeneralTests : BaseManagerUsageTest
    {        
        [Fact]
        public async Task When_A_Method_Wraps_Exceptions_They_Wrap_Successfully()
        {
            Adapter
                .Setup(x => x.GetFileAsync(It.IsAny<GetFileRequest>(), CancellationToken.None))
                .ThrowsAsync(new ExternalException());
            
            Func<Task> func = async () => await FileManager.GetAsync(
                new GetFileRequest
                {
                    FilePath = "abc".AsBaselineFilesystemPath(),
                },
                "default",
                CancellationToken.None
            );
            
            await func
                .Should()
                .ThrowExactlyAsync<AdapterProviderOperationException>()
                .WithMessage("Unhandled exception thrown from adapter (default), potentially whilst communicating with " +
                             "its API. See the inner exception for details.");
        }
    }
}
