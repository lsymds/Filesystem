using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace Storio.Tests.Adapters.S3.Unit.Files
{
    public class WriteTextToFileTests : BaseS3AdapterUnitTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_When_Write_Request_To_S3_Throws_An_Exception()
        {
            S3Client
                .Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), CancellationToken.None))
                .ThrowsAsync(new ArgumentException("foo"));
            
            Func<Task> func = async () => await S3Adapter.WriteTextToFileAsync(
                new WriteTextToFileRequest
                {
                    ContentType = "text/plain",
                    FilePath = "abc".AsStorioPath(),
                    TextToWrite = "foo"
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
