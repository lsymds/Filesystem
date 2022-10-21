using System.Threading;
using System.Threading.Tasks;
using Baseline.Filesystem.Internal.Extensions;
using Baseline.Filesystem.Internal.Validators.Files;

namespace Baseline.Filesystem;

/// <summary>
/// Provides a way to manage files within a number of registered stores.
/// </summary>
public class FileManager : BaseManager, IFileManager
{
    /// <summary>
    /// Initialises a new <see cref="FileManager" /> instance with all of its required dependencies.
    /// </summary>
    /// <param name="storeManager">A store manager implementation.</param>
    public FileManager(IStoreManager storeManager) : base(storeManager) { }

    /// <inheritdoc />
    public async Task<CopyFileResponse> CopyAsync(
        CopyFileRequest copyFileRequest,
        string store = "default",
        CancellationToken cancellationToken = default
    )
    {
        BaseSourceAndDestinationFileRequestValidator.ValidateAndThrowIfUnsuccessful(
            copyFileRequest
        );

        return await GetAdapter(store)
            .CopyFileAsync(
                copyFileRequest.CloneAndCombinePathsWithRootPath(GetStoreRootPath(store)),
                cancellationToken
            )
            .WrapExternalExceptionsAsync(store)
            .RemoveRootPathsAsync(
                r => r.DestinationFile.Path,
                (r, p) => r.DestinationFile.Path = p,
                GetStoreRootPath(store)
            )
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<DeleteFileResponse> DeleteAsync(
        DeleteFileRequest deleteFileRequest,
        string store = "default",
        CancellationToken cancellationToken = default
    )
    {
        BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(deleteFileRequest);

        return await GetAdapter(store)
            .DeleteFileAsync(
                deleteFileRequest.CloneAndCombinePathsWithRootPath(GetStoreRootPath(store)),
                cancellationToken
            )
            .WrapExternalExceptionsAsync(store)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<FileExistsResponse> ExistsAsync(
        FileExistsRequest fileExistsRequest,
        string store = "default",
        CancellationToken cancellationToken = default
    )
    {
        BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(fileExistsRequest);

        return await GetAdapter(store)
            .FileExistsAsync(
                fileExistsRequest.CloneAndCombinePathsWithRootPath(GetStoreRootPath(store)),
                cancellationToken
            )
            .WrapExternalExceptionsAsync(store)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<GetFileResponse> GetAsync(
        GetFileRequest getFileRequest,
        string store = "default",
        CancellationToken cancellationToken = default
    )
    {
        BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(getFileRequest);

        return await GetAdapter(store)
            .GetFileAsync(
                getFileRequest.CloneAndCombinePathsWithRootPath(GetStoreRootPath(store)),
                cancellationToken
            )
            .WrapExternalExceptionsAsync(store)
            .RemoveRootPathsAsync(
                r => r.File.Path,
                (r, p) => r.File.Path = p,
                GetStoreRootPath(store)
            )
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<GetFilePublicUrlResponse> GetPublicUrlAsync(
        GetFilePublicUrlRequest getFilePublicUrlRequest,
        string store = "default",
        CancellationToken cancellationToken = default
    )
    {
        GetFilePublicUrlRequestValidator.ValidateAndThrowIfUnsuccessful(
            getFilePublicUrlRequest
        );

        return await GetAdapter(store)
            .GetFilePublicUrlAsync(
                getFilePublicUrlRequest.CloneAndCombinePathsWithRootPath(
                    GetStoreRootPath(store)
                ),
                cancellationToken
            )
            .WrapExternalExceptionsAsync(store)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<MoveFileResponse> MoveAsync(
        MoveFileRequest moveFileRequest,
        string store = "default",
        CancellationToken cancellationToken = default
    )
    {
        BaseSourceAndDestinationFileRequestValidator.ValidateAndThrowIfUnsuccessful(
            moveFileRequest
        );

        return await GetAdapter(store)
            .MoveFileAsync(
                moveFileRequest.CloneAndCombinePathsWithRootPath(GetStoreRootPath(store)),
                cancellationToken
            )
            .WrapExternalExceptionsAsync(store)
            .RemoveRootPathsAsync(
                r => r.DestinationFile.Path,
                (r, p) => r.DestinationFile.Path = p,
                GetStoreRootPath(store)
            )
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ReadFileAsStreamResponse> ReadAsStreamAsync(
        ReadFileAsStreamRequest readFileAsStreamRequest,
        string store = "default",
        CancellationToken cancellationToken = default
    )
    {
        BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(readFileAsStreamRequest);

        return await GetAdapter(store)
            .ReadFileAsStreamAsync(
                readFileAsStreamRequest.CloneAndCombinePathsWithRootPath(
                    GetStoreRootPath(store)
                ),
                cancellationToken
            )
            .WrapExternalExceptionsAsync(store)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ReadFileAsStringResponse> ReadAsStringAsync(
        ReadFileAsStringRequest readFileAsStringRequest,
        string store = "default",
        CancellationToken cancellationToken = default
    )
    {
        BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(readFileAsStringRequest);

        return await GetAdapter(store)
            .ReadFileAsStringAsync(
                readFileAsStringRequest.CloneAndCombinePathsWithRootPath(
                    GetStoreRootPath(store)
                ),
                cancellationToken
            )
            .WrapExternalExceptionsAsync(store)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<TouchFileResponse> TouchAsync(
        TouchFileRequest touchFileRequest,
        string store = "default",
        CancellationToken cancellationToken = default
    )
    {
        BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(touchFileRequest);

        return await GetAdapter(store)
            .TouchFileAsync(
                touchFileRequest.CloneAndCombinePathsWithRootPath(GetStoreRootPath(store)),
                cancellationToken
            )
            .WrapExternalExceptionsAsync(store)
            .RemoveRootPathsAsync(
                r => r.File.Path,
                (r, p) => r.File.Path = p,
                GetStoreRootPath(store)
            )
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<WriteStreamToFileResponse> WriteStreamAsync(
        WriteStreamToFileRequest writeStreamToFileRequest,
        string store = "default",
        CancellationToken cancellationToken = default
    )
    {
        WriteStreamToFileRequestValidator.ValidateAndThrowIfUnsuccessful(
            writeStreamToFileRequest
        );

        return await GetAdapter(store)
            .WriteStreamToFileAsync(
                writeStreamToFileRequest.CloneAndCombinePathsWithRootPath(
                    GetStoreRootPath(store)
                ),
                cancellationToken
            )
            .WrapExternalExceptionsAsync(store)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<WriteTextToFileResponse> WriteTextAsync(
        WriteTextToFileRequest writeTextToFileRequest,
        string store = "default",
        CancellationToken cancellationToken = default
    )
    {
        WriteTextToFileRequestValidator.ValidateAndThrowIfUnsuccessful(writeTextToFileRequest);

        return await GetAdapter(store)
            .WriteTextToFileAsync(
                writeTextToFileRequest.CloneAndCombinePathsWithRootPath(
                    GetStoreRootPath(store)
                ),
                cancellationToken
            )
            .WrapExternalExceptionsAsync(store)
            .ConfigureAwait(false);
    }
}
