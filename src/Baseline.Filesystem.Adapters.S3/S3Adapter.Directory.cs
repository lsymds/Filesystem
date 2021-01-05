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
        public Task<DirectoryRepresentation> CreateDirectoryAsync(
            CreateDirectoryRequest createDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task DeleteDirectoryAsync(
            DeleteDirectoryRequest deleteDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            await CatchAndWrapProviderExceptions(async () =>
            {
                await CheckDirectoryExistsAsync(deleteDirectoryRequest.DirectoryPath, cancellationToken);
                
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
                );

                return Task.CompletedTask;
            });
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
        /// Checks a directory exists within the adapter's S3 bucket.
        /// </summary>
        /// <param name="directoryPath">The directory path to be checked.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        private async Task CheckDirectoryExistsAsync(
            PathRepresentation directoryPath, 
            CancellationToken cancellationToken
        )
        {
            var files = await ListFilesUnderPath(directoryPath, cancellationToken);

            if (files.S3Objects == null || !files.S3Objects.Any())
            {
                throw new DirectoryNotFoundException(
                    CombineRootAndRequestedPath(directoryPath).NormalisedPath,
                    directoryPath.NormalisedPath
                );
            }
        }
    }
}
