using System.Threading;
using System.Threading.Tasks;

namespace LSymds.Filesystem.Tests.Core.Fixtures;

public class SuccessfulOutcomeAdapter : IAdapter
{
    public Task<CopyDirectoryResponse> CopyDirectoryAsync(
        CopyDirectoryRequest copyDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new CopyDirectoryResponse());
    }

    public Task<CopyFileResponse> CopyFileAsync(
        CopyFileRequest copyFileRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new CopyFileResponse());
    }

    public Task<CreateDirectoryResponse> CreateDirectoryAsync(
        CreateDirectoryRequest createDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new CreateDirectoryResponse());
    }

    public Task<DeleteDirectoryResponse> DeleteDirectoryAsync(
        DeleteDirectoryRequest deleteDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new DeleteDirectoryResponse());
    }

    public Task<DeleteFileResponse> DeleteFileAsync(
        DeleteFileRequest deleteFileRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new DeleteFileResponse());
    }

    public Task<FileExistsResponse> FileExistsAsync(
        FileExistsRequest fileExistsRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new FileExistsResponse { FileExists = true });
    }

    public Task<GetFileResponse> GetFileAsync(
        GetFileRequest getFileRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new GetFileResponse());
    }

    public Task<IterateDirectoryContentsResponse> IterateDirectoryContentsAsync(
        IterateDirectoryContentsRequest iterateDirectoryContentsRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new IterateDirectoryContentsResponse());
    }

    public Task<ListDirectoryContentsResponse> ListDirectoryContentsAsync(
        ListDirectoryContentsRequest listDirectoryContentsRequest,
        CancellationToken cancellationToken = default
    )
    {
        return Task.FromResult(new ListDirectoryContentsResponse());
    }

    public Task<MoveDirectoryResponse> MoveDirectoryAsync(
        MoveDirectoryRequest moveDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new MoveDirectoryResponse());
    }

    public Task<MoveFileResponse> MoveFileAsync(
        MoveFileRequest moveFileRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new MoveFileResponse());
    }

    public Task<ReadFileAsStreamResponse> ReadFileAsStreamAsync(
        ReadFileAsStreamRequest readFileAsStreamRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new ReadFileAsStreamResponse());
    }

    public Task<ReadFileAsStringResponse> ReadFileAsStringAsync(
        ReadFileAsStringRequest readFileAsStringRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new ReadFileAsStringResponse { FileContents = "file contents" });
    }

    public Task<TouchFileResponse> TouchFileAsync(
        TouchFileRequest touchFileRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new TouchFileResponse());
    }

    public Task<WriteStreamToFileResponse> WriteStreamToFileAsync(
        WriteStreamToFileRequest writeStreamToFileRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new WriteStreamToFileResponse());
    }

    public Task<WriteTextToFileResponse> WriteTextToFileAsync(
        WriteTextToFileRequest writeTextToFileRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new WriteTextToFileResponse());
    }

    public Task<GetFilePublicUrlResponse> GetFilePublicUrlAsync(
        GetFilePublicUrlRequest getFilePublicUrlRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(new GetFilePublicUrlResponse());
    }
}
