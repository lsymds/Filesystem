using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace Storio.Tests.Adapters.S3.Unit.Files
{
    public class TouchFileTests : BaseS3AdapterUnitTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_When_Touch_File_Request_To_S3_Throws_An_Exception()
        {
            S3Client
                .Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), CancellationToken.None))
                .ThrowsAsync(new ArgumentException("foo"));

            Func<Task> func = async () => await S3Adapter.TouchFileAsync(
                new TouchFileRequest
                {
                    FilePath = "abc".AsStorioPath(),
                },
                CancellationToken.None
            );
            
            await func
                .Should()
                .ThrowExactlyAsync<AdapterProviderOperationException>()
                .WithMessage("Unexpected exception thrown when communicating with the Amazon S3 endpoint.");
        }
    }
}
