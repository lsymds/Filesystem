using System.Threading;
using System.Threading.Tasks;
using Baseline.Filesystem.Internal.Contracts;

namespace Baseline.Filesystem.Tests.Fixtures
{
    public class SuccessfulOutcomeAdapter : IAdapter
    {
        public Task<DirectoryRepresentation> CopyDirectoryAsync(CopyDirectoryRequest copyDirectoryRequest, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<FileRepresentation> CopyFileAsync(CopyFileRequest copyFileRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FileRepresentation());
        }

        public Task<DirectoryRepresentation> CreateDirectoryAsync(CreateDirectoryRequest createDirectoryRequest, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteDirectoryAsync(DeleteDirectoryRequest deleteDirectoryRequest, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteFileAsync(DeleteFileRequest deleteFileRequest, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<bool> FileExistsAsync(FileExistsRequest fileExistsRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task<FileRepresentation> GetFileAsync(GetFileRequest getFileRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FileRepresentation());
        }

        public Task<DirectoryRepresentation> MoveDirectoryAsync(MoveDirectoryRequest moveDirectoryRequest, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
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

        public Task WriteTextToFileAsync(
            WriteTextToFileRequest writeTextToFileRequest,
            CancellationToken cancellationToken
        )
        {
            return Task.CompletedTask;
        }
    }
}
