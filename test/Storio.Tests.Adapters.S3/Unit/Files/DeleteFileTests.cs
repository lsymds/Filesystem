using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Storio.Tests.Adapters.S3.Unit.Files
{
    public class DeleteFileTests : BaseS3AdapterUnitTest
    {
        [Fact]
        public async Task It_Throws_An_Exception_When_Delete_File_Request_To_S3_Throws_An_Exception()
        {
            S3Client
                .Setup(x => x.DeleteObjectAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("foo"));

            Func<Task> func = async () => await S3Adapter.DeleteFileAsync(
                new DeleteFileRequest
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
