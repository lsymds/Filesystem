using System.Threading;
using System.Threading.Tasks;

namespace Storio.Tests.Fixtures
{
    public class SuccessfulOutcomeAdapter : IAdapter
    {
        public Task<bool> FileExistsAsync(FileExistsRequest fileExistsRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task<FileRepresentation> GetFileAsync(GetFileRequest getFileRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FileRepresentation());
        }

        public Task<FileRepresentation> TouchFileAsync(
            TouchFileRequest touchFileRequest,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(new FileRepresentation());
        }

        public Task WriteTextToFileAsync(
            WriteTextToFileRequest writeTextToFileRequest,
            CancellationToken cancellationToken
        )
        {
            return Task.CompletedTask;
        }
    }
}
