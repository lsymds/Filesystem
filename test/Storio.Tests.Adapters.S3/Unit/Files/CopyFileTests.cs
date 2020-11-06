using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using FluentAssertions;
using Moq;
using Xunit;

namespace Storio.Tests.Adapters.S3.Unit.Files
{
    public class CopyFileTests : BaseS3AdapterUnitTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_When_Copy_File_Request_To_S3_Throws_An_Exception()
        {
            // Configure the S3 client to return that the destination file DOES NOT exist.
            S3Client
                .Setup(
                    x => x.GetObjectMetadataAsync(
                        It.IsAny<string>(),
                        It.Is<string>(y => y == "def"),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ThrowsAsync(new AmazonS3Exception("foo", null, ErrorType.Receiver, "1", "1", HttpStatusCode.NotFound));

            S3Client
                .Setup(
                    x => x.CopyObjectAsync(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()
                    )
                )
                .ThrowsAsync(new ArgumentException("foo"));

            Func<Task> func = async () => await S3Adapter.CopyFileAsync(
                new CopyFileRequest()
                {
                    SourceFilePath = "abc".AsStorioPath(),
                    DestinationFilePath = "def".AsStorioPath()
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
