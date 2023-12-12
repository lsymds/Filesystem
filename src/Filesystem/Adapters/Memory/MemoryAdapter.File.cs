using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LSymds.Filesystem;

/// <summary>
/// Provides the shared, directory/file agnostic functions of the <see cref="IAdapter"/> implementation within memory.
/// Perfect for tests or systems that need short-lived, ephemeral data stores.
/// </summary>
public partial class MemoryAdapter
{
    /// <inheritdoc />
    public async Task<CopyFileResponse> CopyFileAsync(
        CopyFileRequest copyFileRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        ThrowIfFileDoesNotExist(copyFileRequest.SourceFilePath);
        ThrowIfFileExists(copyFileRequest.DestinationFilePath);

        var sourceParentDirectory = _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
            copyFileRequest.SourceFilePath
        );
        var destinationParentDirectory =
            _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
                copyFileRequest.DestinationFilePath
            );

        var fileToCopy = sourceParentDirectory.Files[copyFileRequest.SourceFilePath];

        destinationParentDirectory.Files.Add(copyFileRequest.DestinationFilePath, fileToCopy);

        return new CopyFileResponse
        {
            DestinationFile = new FileRepresentation { Path = copyFileRequest.DestinationFilePath }
        };
    }

    /// <inheritdoc />
    public async Task<DeleteFileResponse> DeleteFileAsync(
        DeleteFileRequest deleteFileRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        ThrowIfFileDoesNotExist(deleteFileRequest.FilePath);

        var parentDirectory = _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
            deleteFileRequest.FilePath
        );

        parentDirectory.Files.Remove(deleteFileRequest.FilePath);

        return new DeleteFileResponse();
    }

    /// <inheritdoc />
    public async Task<FileExistsResponse> FileExistsAsync(
        FileExistsRequest fileExistsRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        return new FileExistsResponse
        {
            FileExists = _configuration.MemoryFilesystem.FileExists(fileExistsRequest.FilePath)
        };
    }

    /// <inheritdoc />
    public async Task<GetFileResponse> GetFileAsync(
        GetFileRequest getFileRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync();

        return _configuration.MemoryFilesystem.FileExists(getFileRequest.FilePath)
            ? new GetFileResponse
            {
                File = new FileRepresentation { Path = getFileRequest.FilePath }
            }
            : null;
    }

    /// <inheritdoc />
    public async Task<GetFilePublicUrlResponse> GetFilePublicUrlAsync(
        GetFilePublicUrlRequest getFilePublicUrlRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        ThrowIfFileDoesNotExist(getFilePublicUrlRequest.FilePath);

        return new GetFilePublicUrlResponse
        {
            Url = _configuration.PublicUrlToReturn,
            Expiry = getFilePublicUrlRequest.Expiry ?? DateTime.Now.AddDays(1)
        };
    }

    /// <inheritdoc />
    public async Task<MoveFileResponse> MoveFileAsync(
        MoveFileRequest moveFileRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        ThrowIfFileDoesNotExist(moveFileRequest.SourceFilePath);
        ThrowIfFileExists(moveFileRequest.DestinationFilePath);

        var sourceParentDirectory = _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
            moveFileRequest.SourceFilePath
        );
        var destinationParentDirectory =
            _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
                moveFileRequest.DestinationFilePath
            );

        var fileToMove = sourceParentDirectory.Files[moveFileRequest.SourceFilePath];

        destinationParentDirectory.Files.Add(moveFileRequest.DestinationFilePath, fileToMove);
        sourceParentDirectory.Files.Remove(moveFileRequest.SourceFilePath);

        return new MoveFileResponse
        {
            DestinationFile = new FileRepresentation { Path = moveFileRequest.DestinationFilePath }
        };
    }

    /// <inheritdoc />
    public async Task<ReadFileAsStreamResponse> ReadFileAsStreamAsync(
        ReadFileAsStreamRequest readFileAsStreamRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        ThrowIfFileDoesNotExist(readFileAsStreamRequest.FilePath);

        var parentDirectory = _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
            readFileAsStreamRequest.FilePath
        );

        var copiedStream = await CopyStreamAsync(parentDirectory.Files[readFileAsStreamRequest.FilePath].Content);
        
        return new ReadFileAsStreamResponse
        {
            FileContents = copiedStream
        };
    }

    /// <inheritdoc />
    public async Task<ReadFileAsStringResponse> ReadFileAsStringAsync(
        ReadFileAsStringRequest readFileAsStringRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        ThrowIfFileDoesNotExist(readFileAsStringRequest.FilePath);

        var parentDirectory = _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
            readFileAsStringRequest.FilePath
        );

        var copiedStream = await CopyStreamAsync(parentDirectory.Files[readFileAsStringRequest.FilePath].Content);
        
        return new ReadFileAsStringResponse
        {
            FileContents = await new StreamReader(copiedStream).ReadToEndAsync()
        };
    }

    /// <inheritdoc />
    public async Task<TouchFileResponse> TouchFileAsync(
        TouchFileRequest touchFileRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        var parentDirectory = _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
            touchFileRequest.FilePath
        );

        parentDirectory.Files.Add(
            touchFileRequest.FilePath,
            new MemoryFileRepresentation(ContentType: "text/plain", new MemoryStream(Encoding.UTF8.GetBytes("")))
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
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);
        
        var parentDirectory = _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
            writeStreamToFileRequest.FilePath
        );

        var copiedStream = await CopyStreamAsync(writeStreamToFileRequest.Stream);

        parentDirectory.Files.Add(
            writeStreamToFileRequest.FilePath,
            new MemoryFileRepresentation(
                ContentType: writeStreamToFileRequest.ContentType,
                Content: copiedStream
            )
        );

        return new WriteStreamToFileResponse();
    }

    /// <inheritdoc />
    public async Task<WriteTextToFileResponse> WriteTextToFileAsync(
        WriteTextToFileRequest writeTextToFileRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        var parentDirectory = _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
            writeTextToFileRequest.FilePath
        );

        parentDirectory.Files.Add(
            writeTextToFileRequest.FilePath,
            new MemoryFileRepresentation(
                ContentType: writeTextToFileRequest.ContentType,
                Content: new MemoryStream(Encoding.UTF8.GetBytes(writeTextToFileRequest.TextToWrite))
            )
        );

        return new WriteTextToFileResponse();
    }

    /// <summary>
    /// Throws a <see cref="FileNotFoundException"/> if a given path does not exist in the filesystem.
    /// </summary>
    private void ThrowIfFileDoesNotExist(PathRepresentation filePath)
    {
        var exists = _configuration.MemoryFilesystem.FileExists(filePath);
        if (!exists)
        {
            throw new FileNotFoundException(filePath.NormalisedPath);
        }
    }

    /// <summary>
    /// Throws a <see cref="FileAlreadyExistsException"/> if a given path already exists in the filesystem.
    /// </summary>
    private void ThrowIfFileExists(PathRepresentation filePath)
    {
        var exists = _configuration.MemoryFilesystem.FileExists(filePath);
        if (exists)
        {
            throw new FileAlreadyExistsException(filePath.NormalisedPath);
        }
    }

    /// <summary>
    /// Copies a stream to another and sets the current read point of both streams back to the start.
    /// </summary>
    private static async Task<Stream> CopyStreamAsync(Stream toCopy)
    {
        var copiedStream = new MemoryStream();
        
        if (toCopy.CanSeek && toCopy.Position != 0)
        {
            toCopy.Seek(0, SeekOrigin.Begin);
        }
        
        await toCopy.CopyToAsync(copiedStream);

        if (copiedStream.CanSeek && copiedStream.Position != 0)
        {
            copiedStream.Seek(0, SeekOrigin.Begin);
        }

        return copiedStream;
    }
}
