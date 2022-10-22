using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Baseline.Filesystem.Internal.Adapters.Memory;

namespace Baseline.Filesystem;

/// <summary>
/// Provides the shared, directory/file agnostic functions of the <see cref="IAdapter"/> implementation within memory.
/// Perfect for tests or systems that need short-lived, ephemeral data stores.
/// </summary>
public partial class MemoryAdapter
{
    /// <inheritdoc />
    public Task<CopyDirectoryResponse> CopyDirectoryAsync(
        CopyDirectoryRequest copyDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<CreateDirectoryResponse> CreateDirectoryAsync(
        CreateDirectoryRequest createDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync();

        ThrowIfDirectoryExists(createDirectoryRequest.DirectoryPath);

        var workingDirectory = _configuration.MemoryFilesystem.RootDirectory;

        foreach (var pathPart in createDirectoryRequest.DirectoryPath.GetPathTree())
        {
            if (workingDirectory.ChildDirectories.ContainsKey(pathPart))
            {
                workingDirectory = workingDirectory.ChildDirectories[pathPart];
                continue;
            }

            workingDirectory.ChildDirectories.Add(
                pathPart,
                new MemoryDirectoryRepresentation(
                    Path: pathPart,
                    ChildDirectories: new Dictionary<
                        PathRepresentation,
                        MemoryDirectoryRepresentation
                    >(),
                    Files: new List<MemoryFileRepresentation>()
                )
            );
        }

        return new CreateDirectoryResponse
        {
            Directory = new DirectoryRepresentation { Path = createDirectoryRequest.DirectoryPath }
        };
    }

    /// <inheritdoc />
    public async Task<DeleteDirectoryResponse> DeleteDirectoryAsync(
        DeleteDirectoryRequest deleteDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync();

        ThrowIfDirectoryDoesNotExist(deleteDirectoryRequest.DirectoryPath);

        var workingDirectory = _configuration.MemoryFilesystem.RootDirectory;

        var pathTree = deleteDirectoryRequest.DirectoryPath.GetPathTree().ToList();
        for (var i = 0; i < pathTree.Count; i++)
        {
            workingDirectory = workingDirectory.ChildDirectories[pathTree[i]];

            // If on the N-1 iteration, remove the key of the Nth element (the full path).
            if (i == pathTree.Count - 2)
            {
                workingDirectory.ChildDirectories.Remove(pathTree[i + 1]);
            }
        }

        return new DeleteDirectoryResponse();
    }

    /// <inheritdoc />
    public Task<IterateDirectoryContentsResponse> IterateDirectoryContentsAsync(
        IterateDirectoryContentsRequest iterateDirectoryContentsRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ListDirectoryContentsResponse> ListDirectoryContentsAsync(
        ListDirectoryContentsRequest listDirectoryContentsRequest,
        CancellationToken cancellationToken = default
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<MoveDirectoryResponse> MoveDirectoryAsync(
        MoveDirectoryRequest moveDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    private void ThrowIfDirectoryExists(PathRepresentation path)
    {
        if (_configuration.MemoryFilesystem.DirectoryExists(path))
        {
            throw new DirectoryAlreadyExistsException(path.NormalisedPath);
        }
    }

    private void ThrowIfDirectoryDoesNotExist(PathRepresentation path)
    {
        if (!_configuration.MemoryFilesystem.DirectoryExists(path))
        {
            throw new DirectoryNotFoundException(path.NormalisedPath);
        }
    }
}
