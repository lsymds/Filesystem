using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Baseline.Filesystem.Internal.Contracts;

namespace Baseline.Filesystem.Adapters.S3
{
    /// <summary>
    /// Provides the directory based functions of an <see cref="IAdapter"/> for Amazon's Simple Storage Service. 
    /// </summary>
    public partial class S3Adapter
    {
        /// <inheritdoc />
        public Task<DirectoryRepresentation> CopyDirectoryAsync(
            CopyDirectoryRequest copyDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<DirectoryRepresentation> CreateDirectoryAsync(
            CreateDirectoryRequest createDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            await CheckDirectoryDoesNotExistAsync(createDirectoryRequest.DirectoryPath, cancellationToken)
                .ConfigureAwait(false);

            // Directories in S3 don't really exist. In order to fake the existence of one, we need to create a
            // temporary file underneath it which causes that path to appear in the S3 browser.
            var pathToCreate = new PathCombinationBuilder(
                createDirectoryRequest.DirectoryPath,
                ".keep".AsBaselineFilesystemPath()
            ).Build();
            
            await TouchFileAsync(new TouchFileRequest {FilePath = pathToCreate}, cancellationToken)
                .ConfigureAwait(false);

            return new DirectoryRepresentation {Path = createDirectoryRequest.DirectoryPath};
        }

        /// <inheritdoc />
        public async Task DeleteDirectoryAsync(
            DeleteDirectoryRequest deleteDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            await CheckDirectoryExistsAsync(deleteDirectoryRequest.DirectoryPath, cancellationToken)
                .ConfigureAwait(false);
            
            await CatchAndWrapProviderExceptions(async () =>
            {
                await ListFilesUnderPathAndPerformActionUntilEmptyAsync(
                     deleteDirectoryRequest.DirectoryPath,
                    response => _s3Client.DeleteObjectsAsync(
                        new DeleteObjectsRequest
                        {
                            BucketName = _adapterConfiguration.BucketName,
                            Objects = response.S3Objects.Select(x => new KeyVersion {Key = x.Key}).ToList()
                        },
                        cancellationToken
                    ),
                    cancellationToken
                ).ConfigureAwait(false);

                return Task.CompletedTask;
            }).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<DirectoryRepresentation> MoveDirectoryAsync(
            MoveDirectoryRequest moveDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Identifies whether a directory (which don't really exist in S3) exists by finding out if there are any
        /// files under a directory styled prefix. Designed to be called outside of a CatchAndWrapProviderExceptions
        /// call.
        /// </summary>
        /// <param name="directoryPath">The directory path to check.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>Whether the 'directory' exists or not.</returns>
        private async Task<bool> DirectoryExistsAsync(
            PathRepresentation directoryPath,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var files = await ListFilesUnderPath(directoryPath, cancellationToken).ConfigureAwait(false);
                return files.S3Objects != null && files.S3Objects.Any();
            }
            catch (Exception e)
            {
                throw ExceptionForS3Exception(e);
            }
        }

        /// <summary>
        /// Checks a directory does not exist within the adapter's S3 bucket, and throws an exception if it does.
        /// </summary>
        /// <param name="directoryPath">The directory path to be checked.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        private async Task CheckDirectoryDoesNotExistAsync(
            PathRepresentation directoryPath,
            CancellationToken cancellationToken
        )
        {
            if (await DirectoryExistsAsync(directoryPath, cancellationToken).ConfigureAwait(false))
            {
                throw new DirectoryAlreadyExistsException(
                    CombineRootAndRequestedPath(directoryPath).NormalisedPath,
                    directoryPath.NormalisedPath
                );
            }
        }

        /// <summary>
        /// Checks a directory exists within the adapter's S3 bucket, and throws an exception if it doesn't.
        /// </summary>
        /// <param name="directoryPath">The directory path to be checked.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        private async Task CheckDirectoryExistsAsync(
            PathRepresentation directoryPath, 
            CancellationToken cancellationToken
        )
        {
            if (!await DirectoryExistsAsync(directoryPath, cancellationToken).ConfigureAwait(false))
            {
                throw new DirectoryNotFoundException(
                    CombineRootAndRequestedPath(directoryPath).NormalisedPath,
                    directoryPath.NormalisedPath
                );
            }
        }
    }
}
