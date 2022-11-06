using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Baseline.Filesystem.Internal.Extensions;

namespace Baseline.Filesystem;

/// <summary>
/// An <see cref="IAdapter"/> implementation for interacting with files and directories on a local disk (or one
/// masquerading as one).
/// </summary>
public partial class LocalAdapter
{
    /// <inheritdoc />
    public async Task<CopyDirectoryResponse> CopyDirectoryAsync(
        CopyDirectoryRequest copyDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        ThrowIfDirectoryDoesNotExist(copyDirectoryRequest.SourceDirectoryPath);
        ThrowIfDirectoryExists(copyDirectoryRequest.DestinationDirectoryPath);

        await ListContentsUnderPathAndPerformActionUntilCompleteAsync(
            copyDirectoryRequest.SourceDirectoryPath,
            pathToCopy =>
            {
                if (pathToCopy.FinalPathPartIsADirectory)
                {
                    Directory.CreateDirectory(
                        pathToCopy
                            .ReplaceDirectoryWithinPath(
                                copyDirectoryRequest.SourceDirectoryPath,
                                copyDirectoryRequest.DestinationDirectoryPath
                            )
                            .NormalisedPath
                    );
                }
                else
                {
                    var pathToCopyTo = pathToCopy.ReplaceDirectoryWithinPath(
                        copyDirectoryRequest.SourceDirectoryPath,
                        copyDirectoryRequest.DestinationDirectoryPath
                    );

                    CreateParentDirectoryForPathIfNotExists(pathToCopyTo);

                    File.Copy(pathToCopy.NormalisedPath, pathToCopyTo.NormalisedPath);
                }

                return Task.FromResult(true);
            }
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
    public async Task<IterateDirectoryContentsResponse> IterateDirectoryContentsAsync(
        IterateDirectoryContentsRequest iterateDirectoryContentsRequest,
        CancellationToken cancellationToken
    )
    {
        ThrowIfDirectoryDoesNotExist(iterateDirectoryContentsRequest.DirectoryPath);

        await ListContentsUnderPathAndPerformActionUntilCompleteAsync(
            iterateDirectoryContentsRequest.DirectoryPath,
            iterateDirectoryContentsRequest.Action
        );

        return new IterateDirectoryContentsResponse();
    }

    /// <inheritdoc />
    public async Task<ListDirectoryContentsResponse> ListDirectoryContentsAsync(
        ListDirectoryContentsRequest listDirectoryContentsRequest,
        CancellationToken cancellationToken = default
    )
    {
        ThrowIfDirectoryDoesNotExist(listDirectoryContentsRequest.DirectoryPath);

        var contents = new HashSet<PathRepresentation>();

        await ListContentsUnderPathAndPerformActionUntilCompleteAsync(
            listDirectoryContentsRequest.DirectoryPath,
            p =>
            {
                contents.Add(p);
                return Task.FromResult(true);
            }
        );

        return new ListDirectoryContentsResponse { Contents = contents.ToList() };
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
        // Recursive function to traverse a directory and add its path, all of its file paths and the results of all of
        // its descendants to the result list.
        async Task TraverseDirectory(PathRepresentation directory)
        {
            var @continue = await action(directory).ConfigureAwait(false);
            if (!@continue)
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(directory.NormalisedPath))
            {
                @continue = await action(SanitiseWindowsPaths(file, false)).ConfigureAwait(false);
                if (!@continue)
                {
                    return;
                }
            }

            foreach (var childDirectory in Directory.EnumerateDirectories(directory.NormalisedPath))
            {
                await TraverseDirectory(SanitiseWindowsPaths(childDirectory, true));
            }
        }

        await TraverseDirectory(path);
    }

    /// <summary>
    /// Windows paths are often returned with backwards slashes in the directory names instead of forward slashes.
    /// This sanitises them to ensure they all work the same.
    /// </summary>
    private PathRepresentation SanitiseWindowsPaths(string path, bool isDirectory)
    {
        if (Path.DirectorySeparatorChar != '\\')
        {
            return isDirectory
                ? $"{path}/".AsBaselineFilesystemPath()
                : path.AsBaselineFilesystemPath();
        }

        var workingPath = new StringBuilder(path).Replace("\\", "/");

        if (isDirectory)
        {
            workingPath.Append('/');
        }

        return workingPath.ToString().AsBaselineFilesystemPath();
    }
}
