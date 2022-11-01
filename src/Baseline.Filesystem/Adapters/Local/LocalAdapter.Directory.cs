using System.IO;
using System.Linq;
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
    public Task<CopyDirectoryResponse> CopyDirectoryAsync(
        CopyDirectoryRequest copyDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        ThrowIfDirectoryDoesNotExist(copyDirectoryRequest.SourceDirectoryPath);
        ThrowIfDirectoryExists(copyDirectoryRequest.DestinationDirectoryPath);

        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<CreateDirectoryResponse> CreateDirectoryAsync(
        CreateDirectoryRequest createDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        ThrowIfDirectoryExists(createDirectoryRequest.DirectoryPath);

        Directory.CreateDirectory(createDirectoryRequest.DirectoryPath.NormalisedPath);

        return Task.FromResult(
            new CreateDirectoryResponse
            {
                Directory = new DirectoryRepresentation
                {
                    Path = createDirectoryRequest.DirectoryPath
                }
            }
        );
    }

    /// <inheritdoc />
    public Task<DeleteDirectoryResponse> DeleteDirectoryAsync(
        DeleteDirectoryRequest deleteDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        ThrowIfDirectoryDoesNotExist(deleteDirectoryRequest.DirectoryPath);

        Directory.Delete(deleteDirectoryRequest.DirectoryPath.NormalisedPath, true);

        return Task.FromResult(new DeleteDirectoryResponse());
    }

    /// <inheritdoc />
    public Task<IterateDirectoryContentsResponse> IterateDirectoryContentsAsync(
        IterateDirectoryContentsRequest iterateDirectoryContentsRequest,
        CancellationToken cancellationToken
    )
    {
        ThrowIfDirectoryDoesNotExist(iterateDirectoryContentsRequest.DirectoryPath);

        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ListDirectoryContentsResponse> ListDirectoryContentsAsync(
        ListDirectoryContentsRequest listDirectoryContentsRequest,
        CancellationToken cancellationToken = default
    )
    {
        ThrowIfDirectoryDoesNotExist(listDirectoryContentsRequest.DirectoryPath);

        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<MoveDirectoryResponse> MoveDirectoryAsync(
        MoveDirectoryRequest moveDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        ThrowIfDirectoryDoesNotExist(moveDirectoryRequest.SourceDirectoryPath);
        ThrowIfDirectoryExists(moveDirectoryRequest.DestinationDirectoryPath);

        CreateParentDirectoryForPathIfNotExists(moveDirectoryRequest.DestinationDirectoryPath);

        Directory.Move(
            moveDirectoryRequest.SourceDirectoryPath.NormalisedPath,
            moveDirectoryRequest.DestinationDirectoryPath.NormalisedPath
        );

        return Task.FromResult(
            new MoveDirectoryResponse
            {
                DestinationDirectory = new DirectoryRepresentation
                {
                    Path = moveDirectoryRequest.DestinationDirectoryPath
                }
            }
        );
    }

    /// <summary>
    /// Creates the parent directory and any of its parent directories for a given path.
    /// </summary>
    private static void CreateParentDirectoryForPathIfNotExists(PathRepresentation path)
    {
        var pathTree = path.GetPathTree().ToList();

        // If only the file is in the path, then the directory it is in MUST exist.
        if (pathTree.Count == 1)
        {
            return;
        }

        var parentDirectory = pathTree[^2];
        Directory.CreateDirectory(parentDirectory.NormalisedPath);
    }

    /// <summary>
    /// Throws a <see cref="DirectoryAlreadyExistsException"/> if a given directory path exists.
    /// </summary>
    private static void ThrowIfDirectoryExists(PathRepresentation path)
    {
        if (Directory.Exists(path.NormalisedPath))
        {
            throw new DirectoryAlreadyExistsException(path.NormalisedPath);
        }
    }

    /// <summary>
    /// Throws a <see cref="DirectoryNotFoundException"/> if a given directory path does not exist.
    /// </summary>
    private static void ThrowIfDirectoryDoesNotExist(PathRepresentation path)
    {
        if (!Directory.Exists(path.NormalisedPath))
        {
            throw new DirectoryNotFoundException(path.NormalisedPath);
        }
    }
}
