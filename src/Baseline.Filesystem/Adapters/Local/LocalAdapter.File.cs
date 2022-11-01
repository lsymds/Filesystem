using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Baseline.Filesystem;

/// <summary>
/// An <see cref="IAdapter"/> implementation for interacting with files and directories on a local disk (or one
/// masquerading as one).
/// </summary>
public partial class LocalAdapter
{
    /// <inheritdoc />
    public Task<CopyFileResponse> CopyFileAsync(
        CopyFileRequest copyFileRequest,
        CancellationToken cancellationToken
    )
    {
        ThrowIfFileDoesNotExist(copyFileRequest.SourceFilePath);
        ThrowIfFileExists(copyFileRequest.DestinationFilePath);

        File.Copy(
            copyFileRequest.SourceFilePath.NormalisedPath,
            copyFileRequest.DestinationFilePath.NormalisedPath
        );

        return Task.FromResult(
            new CopyFileResponse
            {
                DestinationFile = new FileRepresentation
                {
                    Path = copyFileRequest.DestinationFilePath
                }
            }
        );
    }

    /// <inheritdoc />
    public Task<DeleteFileResponse> DeleteFileAsync(
        DeleteFileRequest deleteFileRequest,
        CancellationToken cancellationToken
    )
    {
        ThrowIfFileDoesNotExist(deleteFileRequest.FilePath);

        File.Delete(deleteFileRequest.FilePath.NormalisedPath);

        return Task.FromResult(new DeleteFileResponse());
    }

    /// <inheritdoc />
    public Task<FileExistsResponse> FileExistsAsync(
        FileExistsRequest fileExistsRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(
            new FileExistsResponse
            {
                FileExists = File.Exists(fileExistsRequest.FilePath.NormalisedPath)
            }
        );
    }

    /// <inheritdoc />
    public Task<GetFileResponse> GetFileAsync(
        GetFileRequest getFileRequest,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(
            FileExists(getFileRequest.FilePath)
                ? new GetFileResponse
                {
                    File = new FileRepresentation { Path = getFileRequest.FilePath }
                }
                : null
        );
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
        ThrowIfFileDoesNotExist(moveFileRequest.SourceFilePath);
        ThrowIfFileExists(moveFileRequest.DestinationFilePath);

        File.Move(
            moveFileRequest.SourceFilePath.NormalisedPath,
            moveFileRequest.DestinationFilePath.NormalisedPath
        );

        return Task.FromResult(
            new MoveFileResponse
            {
                DestinationFile = new FileRepresentation
                {
                    Path = moveFileRequest.DestinationFilePath
                }
            }
        );
    }

    /// <inheritdoc />
    public Task<ReadFileAsStreamResponse> ReadFileAsStreamAsync(
        ReadFileAsStreamRequest readFileAsStreamRequest,
        CancellationToken cancellationToken
    )
    {
        ThrowIfFileDoesNotExist(readFileAsStreamRequest.FilePath);

        return Task.FromResult(
            new ReadFileAsStreamResponse
            {
                FileContents = File.OpenRead(readFileAsStreamRequest.FilePath.NormalisedPath)
            }
        );
    }

    /// <inheritdoc />
    public async Task<ReadFileAsStringResponse> ReadFileAsStringAsync(
        ReadFileAsStringRequest readFileAsStringRequest,
        CancellationToken cancellationToken
    )
    {
        ThrowIfFileDoesNotExist(readFileAsStringRequest.FilePath);

        return new ReadFileAsStringResponse
        {
            FileContents = await File.ReadAllTextAsync(
                readFileAsStringRequest.FilePath.NormalisedPath,
                cancellationToken
            )
        };
    }

    /// <inheritdoc />
    public async Task<TouchFileResponse> TouchFileAsync(
        TouchFileRequest touchFileRequest,
        CancellationToken cancellationToken
    )
    {
        ThrowIfFileExists(touchFileRequest.FilePath);

        await File.WriteAllTextAsync(
            touchFileRequest.FilePath.NormalisedPath,
            "",
            cancellationToken
        );

        return new TouchFileResponse
        {
            File = new FileRepresentation { Path = touchFileRequest.FilePath }
        };
    }

    /// <inheritdoc />
    public async Task<WriteStreamToFileResponse> WriteStreamToFileAsync(
        WriteStreamToFileRequest writeStreamToFileRequest,
        CancellationToken cancellationToken
    )
    {
        await using var destinationFile = File.OpenWrite(
            writeStreamToFileRequest.FilePath.NormalisedPath
        );

        await writeStreamToFileRequest.Stream.CopyToAsync(destinationFile, cancellationToken);

        await destinationFile.FlushAsync(cancellationToken);

        return new WriteStreamToFileResponse();
    }

    /// <inheritdoc />
    public async Task<WriteTextToFileResponse> WriteTextToFileAsync(
        WriteTextToFileRequest writeTextToFileRequest,
        CancellationToken cancellationToken
    )
    {
        await File.WriteAllTextAsync(
            writeTextToFileRequest.FilePath.NormalisedPath,
            writeTextToFileRequest.TextToWrite,
            cancellationToken
        );

        return new WriteTextToFileResponse();
    }

    /// <summary>
    /// Returns whether or not the given file exists.
    /// </summary>
    private static bool FileExists(PathRepresentation filePath)
    {
        return File.Exists(filePath.NormalisedPath);
    }

    /// <summary>
    /// Throws a <see cref="FileAlreadyExistsException"/> if the given path already exists.
    /// </summary>
    private static void ThrowIfFileExists(PathRepresentation filePath)
    {
        if (FileExists(filePath))
        {
            throw new FileAlreadyExistsException(filePath.NormalisedPath);
        }
    }

    /// <summary>
    /// Throws a <see cref="FileNotFoundException"/> if the given path does not exist.
    /// </summary>
    private static void ThrowIfFileDoesNotExist(PathRepresentation filePath)
    {
        if (!FileExists(filePath))
        {
            throw new FileNotFoundException(filePath.NormalisedPath);
        }
    }
}
