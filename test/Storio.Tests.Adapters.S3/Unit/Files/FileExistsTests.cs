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
    public class FileExistsTests : BaseS3AdapterUnitTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_When_File_Exists_Request_To_S3_Throws_An_Exception()
        {
            S3Client
                .Setup(x => x.GetObjectMetadataAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("foo"));

            Func<Task> func = async () => await S3Adapter.FileExistsAsync(
                new FileExistsRequest
                {
                    FilePath = "abc".AsStorioPath(),
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
