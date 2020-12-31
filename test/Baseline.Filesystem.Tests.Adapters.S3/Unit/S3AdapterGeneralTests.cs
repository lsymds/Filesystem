using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Unit
{
    public class S3AdapterGeneralTests : BaseS3AdapterUnitTest
    {        
        [Fact]
        public async Task When_A_Method_Wraps_Exceptions_They_Wrap_Successfully()
        {
            S3Client
                .Setup(x => x.GetObjectMetadataAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("foo"));

            Func<Task> func = async () => await S3Adapter.GetFileAsync(
                new GetFileRequest
                {
                    FilePath = "abc".AsBaselineFilesystemPath(),
                },
                CancellationToken.None
            );
            
            await func
                .Should()
                .ThrowExactlyAsync<AdapterProviderOperationException>()
                .WithMessage("Unexpected exception thrown when communicating with the Amazon S3 endpoint. " +
                             "See inner exception for details.");
        }
    }
}
