using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Baseline.Filesystem.Adapters.S3.Internal.Extensions;
using Baseline.Filesystem.Internal.Extensions;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Provides the directory based functions of an <see cref="IAdapter"/> for Amazon's Simple Storage Service. 
    /// </summary>
    public partial class S3Adapter
    {
        /// <inheritdoc />
        public async Task<CopyDirectoryResponse> CopyDirectoryAsync(
            CopyDirectoryRequest copyDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            await EnsureDirectoryExistsAsync(copyDirectoryRequest.SourceDirectoryPath, cancellationToken)
                .ConfigureAwait(false);
            await EnsureDirectoryDoesNotExistAsync(copyDirectoryRequest.DestinationDirectoryPath, cancellationToken)
                .ConfigureAwait(false);

            await CopyDirectoryInternalAsync(
                copyDirectoryRequest.SourceDirectoryPath,
                copyDirectoryRequest.DestinationDirectoryPath,
                cancellationToken
            ).ConfigureAwait(false);

            return new CopyDirectoryResponse
            {
                DestinationDirectory = new DirectoryRepresentation {Path = copyDirectoryRequest.DestinationDirectoryPath}
            };
        }

        /// <inheritdoc />
        public async Task<CreateDirectoryResponse> CreateDirectoryAsync(
            CreateDirectoryRequest createDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            await EnsureDirectoryDoesNotExistAsync(createDirectoryRequest.DirectoryPath, cancellationToken)
                .ConfigureAwait(false);

            var pathToCreate = new PathCombinationBuilder(
                createDirectoryRequest.DirectoryPath,
                ".keep".AsBaselineFilesystemPath()
            ).Build();

            await TouchFileInternalAsync(pathToCreate, cancellationToken).ConfigureAwait(false);

            return new CreateDirectoryResponse
            {
                Directory = new DirectoryRepresentation {Path = createDirectoryRequest.DirectoryPath}
            };
        }

        /// <inheritdoc />
        public async Task<DeleteDirectoryResponse> DeleteDirectoryAsync(
            DeleteDirectoryRequest deleteDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            await EnsureDirectoryExistsAsync(deleteDirectoryRequest.DirectoryPath, cancellationToken)
                .ConfigureAwait(false);

            await ListAndReturnPaginatedFilesUnderPathAndPerformActionUntilCompleteAsync(
                deleteDirectoryRequest.DirectoryPath,
                async response => await _s3Client.DeleteObjectsAsync(
                    new DeleteObjectsRequest
                    {
                        BucketName = _adapterConfiguration.BucketName,
                        Objects = response.S3Objects.Select(x => new KeyVersion {Key = x.Key}).ToList()
                    },
                    cancellationToken
                ).ConfigureAwait(false),
                cancellationToken
            ).ConfigureAwait(false);

            return new DeleteDirectoryResponse();
        }

        /// <inheritdoc />
        public async Task<IterateDirectoryContentsResponse> IterateDirectoryContentsAsync(
            IterateDirectoryContentsRequest iterateDirectoryContentsRequest,
            CancellationToken cancellationToken
        )
        {
            await EnsureDirectoryExistsAsync(iterateDirectoryContentsRequest.DirectoryPath, cancellationToken)
                .ConfigureAwait(false);

            await ListPaginatedFilesUnderPathAndPerformActionUntilCompleteAsync(
                iterateDirectoryContentsRequest.DirectoryPath,
                async r =>
                {
                    var pathTracker = new Dictionary<string, PathRepresentation>();
                    foreach (var file in r.S3Objects)
                    {
                        var tree = BuildOrderedPathTree(file.Key.AsBaselineFilesystemPath(), pathTracker);
                        foreach (var treeItem in tree)
                        {
                            await iterateDirectoryContentsRequest.Action(treeItem);    
                        }
                    }
                },
                cancellationToken
            ).ConfigureAwait(false);

            return new IterateDirectoryContentsResponse();
        }

        /// <inheritdoc />
        public async Task<ListDirectoryContentsResponse> ListDirectoryContentsAsync(
            ListDirectoryContentsRequest listDirectoryContentsRequest,
            CancellationToken cancellationToken = default
        )
        {
            await EnsureDirectoryExistsAsync(listDirectoryContentsRequest.DirectoryPath, cancellationToken)
                .ConfigureAwait(false);

            var directorysContents = new List<PathRepresentation>();
            var pathTracker = new Dictionary<string, PathRepresentation>();

            await ListPaginatedFilesUnderPathAndPerformActionUntilCompleteAsync(
                listDirectoryContentsRequest.DirectoryPath,
                r =>
                {
                    r.S3Objects.ForEach(
                        o => directorysContents.AddRange(
                            BuildOrderedPathTree(o.Key.AsBaselineFilesystemPath(), pathTracker)
                        )
                    );
                    return Task.CompletedTask;
                },
                cancellationToken
            ).ConfigureAwait(false);

            return new ListDirectoryContentsResponse {Contents = directorysContents};
        }

        /// <inheritdoc />
        public async Task<MoveDirectoryResponse> MoveDirectoryAsync(
            MoveDirectoryRequest moveDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            await EnsureDirectoryExistsAsync(moveDirectoryRequest.SourceDirectoryPath, cancellationToken)
                .ConfigureAwait(false);
            await EnsureDirectoryDoesNotExistAsync(moveDirectoryRequest.DestinationDirectoryPath, cancellationToken)
                .ConfigureAwait(false);

            var sourceFiles = await CopyDirectoryInternalAsync(
                moveDirectoryRequest.SourceDirectoryPath,
                moveDirectoryRequest.DestinationDirectoryPath,
                cancellationToken
            ).ConfigureAwait(false);

            await Task.WhenAll(
                sourceFiles.ChunkBy(1000).Select(
                    async x => await _s3Client.DeleteObjectsAsync(
                        new DeleteObjectsRequest
                        {
                            BucketName = _adapterConfiguration.BucketName,
                            Objects = x.Select(y => new KeyVersion {Key = y.Key}).ToList(),
                        },
                        cancellationToken
                    ).ConfigureAwait(false)
                )
            ).ConfigureAwait(false);

            return new MoveDirectoryResponse
            {
                DestinationDirectory = new DirectoryRepresentation
                {
                    Path = moveDirectoryRequest.DestinationDirectoryPath
                }
            };
        }

        /// <summary>
        /// Identifies whether a directory (which don't really exist in S3) exists by finding out if there are any
        /// files under a directory styled prefix. 
        /// </summary>
        /// <param name="directoryPath">The directory path to check.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>Whether the 'directory' exists or not.</returns>
        private async Task<bool> DirectoryExistsAsync(
            PathRepresentation directoryPath,
            CancellationToken cancellationToken
        )
        {
            var files = await ListFilesUnderPath(directoryPath, cancellationToken).ConfigureAwait(false);
            return files.S3Objects != null && files.S3Objects.Any();
        }

        /// <summary>
        /// Checks a directory does not exist within the store's S3 bucket, and throws an exception if it does.
        /// </summary>
        /// <param name="directoryPath">The directory path to be checked.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        private async Task EnsureDirectoryDoesNotExistAsync(
            PathRepresentation directoryPath,
            CancellationToken cancellationToken
        )
        {
            if (await DirectoryExistsAsync(directoryPath, cancellationToken).ConfigureAwait(false))
            {
                throw new DirectoryAlreadyExistsException(directoryPath.NormalisedPath);
            }
        }

        /// <summary>
        /// Checks a directory exists within the store's S3 bucket, and throws an exception if it doesn't.
        /// </summary>
        /// <param name="directoryPath">The directory path to be checked.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        private async Task EnsureDirectoryExistsAsync(
            PathRepresentation directoryPath,
            CancellationToken cancellationToken
        )
        {
            if (!await DirectoryExistsAsync(directoryPath, cancellationToken).ConfigureAwait(false))
            {
                throw new DirectoryNotFoundException(directoryPath.NormalisedPath);
            }
        }

        /// <summary>
        /// Internal method to copy a directory from a source path to a destination path.
        /// </summary>
        /// <param name="sourcePath">The source path to copy from.</param>
        /// <param name="destinationPath">The destination path to copy to.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The collection containing all of the copied files.</returns>
        private async Task<List<S3Object>> CopyDirectoryInternalAsync(
            PathRepresentation sourcePath,
            PathRepresentation destinationPath,
            CancellationToken cancellationToken
        )
        {
            return await ListAndReturnPaginatedFilesUnderPathAndPerformActionUntilCompleteAsync(
                sourcePath,
                async objects =>
                {
                    foreach (var obj in objects.S3Objects)
                    {
                        var newFileLocation = obj.Key.ReplaceFirstOccurrence(
                            sourcePath.S3SafeDirectoryPath(),
                            destinationPath.S3SafeDirectoryPath()
                        );

                        await _s3Client.CopyObjectAsync(
                            _adapterConfiguration.BucketName,
                            obj.Key,
                            _adapterConfiguration.BucketName,
                            newFileLocation,
                            cancellationToken
                        ).ConfigureAwait(false);
                    }
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Builds and returns an ordered path tree from a path returned from the S3 API.
        /// </summary>
        /// <param name="basePath">The path to create a tree from.</param>
        /// <param name="directoryPathTracker">A collection of paths that have already been executed.</param>
        private IEnumerable<PathRepresentation> BuildOrderedPathTree(
            PathRepresentation basePath,
            Dictionary<string, PathRepresentation> directoryPathTracker
        )
        {
            var addedDirectoryPaths = directoryPathTracker ?? new Dictionary<string, PathRepresentation>();
            
            if (basePath.OriginalPath.Contains("/"))
            {
                foreach (var p in basePath.GetPathTree())
                {
                    if (addedDirectoryPaths.ContainsKey(p.NormalisedPath))
                    {
                        continue;
                    }

                    addedDirectoryPaths.Add(p.NormalisedPath, p);
                    yield return p;
                }
            }

            yield return basePath;
        }
    }
}
