using System.Threading;
using System.Threading.Tasks;

namespace Baseline.Filesystem.Tests.Fixtures
{
    public class SuccessfulOutcomeAdapter : IAdapter
    {
        public Task<DirectoryRepresentation> CopyDirectoryAsync(CopyDirectoryRequest copyDirectoryRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(new DirectoryRepresentation());
        }

        public Task<CopyFileResponse> CopyFileAsync(CopyFileRequest copyFileRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CopyFileResponse());
        }

        public Task<DirectoryRepresentation> CreateDirectoryAsync(CreateDirectoryRequest createDirectoryRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(new DirectoryRepresentation());
        }

        public Task DeleteDirectoryAsync(DeleteDirectoryRequest deleteDirectoryRequest, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task DeleteFileAsync(DeleteFileRequest deleteFileRequest, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<FileExistsResponse> FileExistsAsync(FileExistsRequest fileExistsRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FileExistsResponse { FileExists = true });
        }

        public Task<FileRepresentation> GetFileAsync(GetFileRequest getFileRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FileRepresentation());
        }

        public Task<ListDirectoryContentsResponse> ListDirectoryContentsAsync(ListDirectoryContentsRequest listDirectoryContentsRequest,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new ListDirectoryContentsResponse());
        }

        public Task<DirectoryRepresentation> MoveDirectoryAsync(MoveDirectoryRequest moveDirectoryRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(new DirectoryRepresentation());
        }

        public Task<FileRepresentation> MoveFileAsync(MoveFileRequest moveFileRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FileRepresentation());
        }

        public Task<string> ReadFileAsStringAsync(
            ReadFileAsStringRequest readFileAsStringRequest,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult("file contents");
        }

        public Task<FileRepresentation> TouchFileAsync(
            TouchFileRequest touchFileRequest,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(new FileRepresentation());
        }

        public Task<WriteStreamToFileResponse> WriteStreamToFileAsync(
            WriteStreamToFileRequest writeStreamToFileRequest,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(new WriteStreamToFileResponse());
        }

        public Task WriteTextToFileAsync(
            WriteTextToFileRequest writeTextToFileRequest,
            CancellationToken cancellationToken
        )
        {
            return Task.CompletedTask;
        }

        public Task<GetFilePublicUrlResponse> GetFilePublicUrlAsync(
            GetFilePublicUrlRequest getFilePublicUrlRequest,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(new GetFilePublicUrlResponse());
        }
    }
}
