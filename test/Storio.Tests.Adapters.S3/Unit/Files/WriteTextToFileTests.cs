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
    public class WriteTextToFileTests : BaseS3AdapterUnitTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_When_Write_Request_Is_Not_Successful()
        {
            S3Client
                .Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), CancellationToken.None))
                .ReturnsAsync(new PutObjectResponse
                {
                    HttpStatusCode = HttpStatusCode.Forbidden
                });

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
                .ThrowExactlyAsync<WriteTextToFileException>()
                .WithMessage("HttpStatusCode response from AWS S3 was not successful (received Forbidden).");
        }
    }
}
