using System.Threading;
using System.Threading.Tasks;

namespace Baseline.Filesystem;

/// <summary>
/// Provides the shared, directory/file agnostic functions of the <see cref="IAdapter"/> implementation within memory.
/// Perfect for tests or systems that need short-lived, ephemeral data stores.
/// </summary>
public partial class MemoryAdapter
{
    /// <inheritdoc />
    public Task<CopyFileResponse> CopyFileAsync(
        CopyFileRequest copyFileRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<DeleteFileResponse> DeleteFileAsync(
        DeleteFileRequest deleteFileRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<FileExistsResponse> FileExistsAsync(
        FileExistsRequest fileExistsRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<GetFileResponse> GetFileAsync(
        GetFileRequest getFileRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<GetFilePublicUrlResponse> GetFilePublicUrlAsync(
        GetFilePublicUrlRequest getFilePublicUrlRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<MoveFileResponse> MoveFileAsync(
        MoveFileRequest moveFileRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ReadFileAsStreamResponse> ReadFileAsStreamAsync(
        ReadFileAsStreamRequest readFileAsStreamRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ReadFileAsStringResponse> ReadFileAsStringAsync(
        ReadFileAsStringRequest readFileAsStringRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<TouchFileResponse> TouchFileAsync(
        TouchFileRequest touchFileRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<WriteStreamToFileResponse> WriteStreamToFileAsync(
        WriteStreamToFileRequest writeStreamToFileRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<WriteTextToFileResponse> WriteTextToFileAsync(
        WriteTextToFileRequest writeTextToFileRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }
}
