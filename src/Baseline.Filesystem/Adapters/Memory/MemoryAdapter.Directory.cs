using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Baseline.Filesystem.Internal.Extensions;

namespace Baseline.Filesystem;

/// <summary>
/// Provides the shared, directory/file agnostic functions of the <see cref="IAdapter"/> implementation within memory.
/// Perfect for tests or systems that need short-lived, ephemeral data stores.
/// </summary>
public partial class MemoryAdapter
{
    /// <inheritdoc />
    public async Task<CopyDirectoryResponse> CopyDirectoryAsync(
        CopyDirectoryRequest copyDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        ThrowIfDirectoryDoesNotExist(copyDirectoryRequest.SourceDirectoryPath);
        ThrowIfDirectoryExists(copyDirectoryRequest.DestinationDirectoryPath);

        var parentOfSourceDirectory = _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
            copyDirectoryRequest.SourceDirectoryPath
        );
        var parentOfDestinationDirectory =
            _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
                copyDirectoryRequest.DestinationDirectoryPath
            );

        // Take the original directory but deep clone any mutable types (i.e. dictionaries).
        var directoryToCopy = parentOfSourceDirectory.ChildDirectories[
            copyDirectoryRequest.SourceDirectoryPath
        ].DeepCloneMutableTypes();

        parentOfDestinationDirectory.ChildDirectories.Add(
            copyDirectoryRequest.DestinationDirectoryPath,
            directoryToCopy
        );

        TraverseDirectoryAndRewritePaths(
            directoryToCopy,
            copyDirectoryRequest.SourceDirectoryPath,
            copyDirectoryRequest.DestinationDirectoryPath
        );

        return new CopyDirectoryResponse
        {
            DestinationDirectory = new DirectoryRepresentation
            {
                Path = copyDirectoryRequest.DestinationDirectoryPath
            }
        };
    }

    /// <inheritdoc />
    public async Task<CreateDirectoryResponse> CreateDirectoryAsync(
        CreateDirectoryRequest createDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        ThrowIfDirectoryExists(createDirectoryRequest.DirectoryPath);

        _configuration.MemoryFilesystem.GetOrCreateDirectory(createDirectoryRequest.DirectoryPath);

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
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        ThrowIfDirectoryDoesNotExist(deleteDirectoryRequest.DirectoryPath);

        var parentDirectoryOfDirectoryToDelete =
            _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
                deleteDirectoryRequest.DirectoryPath
            );

        parentDirectoryOfDirectoryToDelete.ChildDirectories.Remove(
            deleteDirectoryRequest.DirectoryPath
        );

        return new DeleteDirectoryResponse();
    }

    /// <inheritdoc />
    public async Task<IterateDirectoryContentsResponse> IterateDirectoryContentsAsync(
        IterateDirectoryContentsRequest iterateDirectoryContentsRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        ThrowIfDirectoryDoesNotExist(iterateDirectoryContentsRequest.DirectoryPath);

        await ListContentsUnderPathAndPerformActionUntilCompleteAsync(
                iterateDirectoryContentsRequest.DirectoryPath,
                iterateDirectoryContentsRequest.Action
            )
            .ConfigureAwait(false);

        return new IterateDirectoryContentsResponse();
    }

    /// <inheritdoc />
    public async Task<ListDirectoryContentsResponse> ListDirectoryContentsAsync(
        ListDirectoryContentsRequest listDirectoryContentsRequest,
        CancellationToken cancellationToken = default
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        ThrowIfDirectoryDoesNotExist(listDirectoryContentsRequest.DirectoryPath);

        var results = new List<PathRepresentation>();

        await ListContentsUnderPathAndPerformActionUntilCompleteAsync(
                listDirectoryContentsRequest.DirectoryPath,
                path =>
                {
                    results.Add(path);
                    return Task.FromResult(true);
                }
            )
            .ConfigureAwait(false);

        return new ListDirectoryContentsResponse { Contents = results };
    }

    /// <inheritdoc />
    public async Task<MoveDirectoryResponse> MoveDirectoryAsync(
        MoveDirectoryRequest moveDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        using var _ = await LockFilesystemAsync().ConfigureAwait(false);

        ThrowIfDirectoryDoesNotExist(moveDirectoryRequest.SourceDirectoryPath);
        ThrowIfDirectoryExists(moveDirectoryRequest.DestinationDirectoryPath);

        var parentOfSourceDirectory = _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
            moveDirectoryRequest.SourceDirectoryPath
        );
        var parentOfDestinationDirectory =
            _configuration.MemoryFilesystem.GetOrCreateParentDirectoryOf(
                moveDirectoryRequest.DestinationDirectoryPath
            );

        var directoryToMove = parentOfSourceDirectory.ChildDirectories[
            moveDirectoryRequest.SourceDirectoryPath
        ];

        parentOfDestinationDirectory.ChildDirectories.Add(
            moveDirectoryRequest.DestinationDirectoryPath,
            directoryToMove
        );
        parentOfSourceDirectory.ChildDirectories.Remove(moveDirectoryRequest.SourceDirectoryPath);

        // Recursively traverse the destination directory and replaces any source paths for child directories or files
        // with that of the new destination directory.
        TraverseDirectoryAndRewritePaths(
            parentOfDestinationDirectory,
            moveDirectoryRequest.SourceDirectoryPath,
            moveDirectoryRequest.DestinationDirectoryPath
        );

        return new MoveDirectoryResponse
        {
            DestinationDirectory = new DirectoryRepresentation
            {
                Path = moveDirectoryRequest.DestinationDirectoryPath
            }
        };
    }

    /// <summary>
    /// Lists all of the contents under a path (including the provided path itself) and performs a provided action
    /// until either a) all contents have been actioned or b) an action returns false indicating the listing should
    /// stop.
    /// </summary>
    private async Task ListContentsUnderPathAndPerformActionUntilCompleteAsync(
        PathRepresentation path,
        Func<PathRepresentation, Task<bool>> action
    )
    {
        var directoryToListContentsOf = _configuration.MemoryFilesystem.GetOrCreateDirectory(path);

        // Recursive function to traverse a directory and add its path, all of its file paths and the results of all of
        // its descendants to the result list.
        async Task TraverseDirectory(
            (PathRepresentation Path, MemoryDirectoryRepresentation Representation) directory
        )
        {
            var @continue = await action(directory.Path).ConfigureAwait(false);
            if (!@continue)
            {
                return;
            }

            foreach (var file in directory.Representation.Files.Keys)
            {
                @continue = await action(file).ConfigureAwait(false);
                if (!@continue)
                {
                    return;
                }
            }

            foreach (var childDirectory in directory.Representation.ChildDirectories)
            {
                await TraverseDirectory((childDirectory.Key, childDirectory.Value));
            }
        }

        await TraverseDirectory((path, directoryToListContentsOf));
    }

    /// <summary>
    /// Throws a <see cref="DirectoryAlreadyExistsException"/> if a given directory path already exists.
    /// </summary>
    private void ThrowIfDirectoryExists(PathRepresentation path)
    {
        if (_configuration.MemoryFilesystem.DirectoryExists(path))
        {
            throw new DirectoryAlreadyExistsException(path.NormalisedPath);
        }
    }

    /// <summary>
    /// Throws a <see cref="DirectoryNotFoundException"/> if a given directory path does not already.
    /// </summary>
    private void ThrowIfDirectoryDoesNotExist(PathRepresentation path)
    {
        if (!_configuration.MemoryFilesystem.DirectoryExists(path))
        {
            throw new DirectoryNotFoundException(path.NormalisedPath);
        }
    }

    /// <summary>
    /// Given a directory within a filesystem, recursively traverse it and replace any paths matching
    /// originalPath with the replacementPath.
    /// </summary>
    private void TraverseDirectoryAndRewritePaths(
        MemoryDirectoryRepresentation directoryRepresentation,
        PathRepresentation originalPath,
        PathRepresentation replacementPath
    )
    {
        foreach (var file in directoryRepresentation.Files.ToList())
        {
            var newPath = file.Key.ReplaceDirectoryWithinPath(originalPath, replacementPath);

            directoryRepresentation.Files.Remove(file.Key);
            directoryRepresentation.Files.Add(newPath, file.Value);
        }

        foreach (var childDirectory in directoryRepresentation.ChildDirectories.ToList())
        {
            var newPath = childDirectory.Key.ReplaceDirectoryWithinPath(
                originalPath,
                replacementPath
            );

            directoryRepresentation.ChildDirectories.Remove(childDirectory.Key);
            directoryRepresentation.ChildDirectories.Add(newPath, childDirectory.Value);

            TraverseDirectoryAndRewritePaths(childDirectory.Value, originalPath, replacementPath);
        }
    }
}
