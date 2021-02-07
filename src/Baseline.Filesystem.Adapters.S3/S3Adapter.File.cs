using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Baseline.Filesystem.Adapters.S3.Internal.Extensions;

namespace Baseline.Filesystem.Adapters.S3
{
    /// <summary>
    /// Provides the file based functions of an <see cref="IAdapter"/> for Amazon's Simple Storage Service. 
    /// </summary>
    public partial class S3Adapter
    {
        /// <inheritdoc />
        public async Task<FileRepresentation> CopyFileAsync(
            CopyFileRequest copyFileRequest, 
            CancellationToken cancellationToken
        )
        {
            await EnsureFileExistsAsync(copyFileRequest.SourceFilePath, cancellationToken).ConfigureAwait(false);
            await EnsureFileDoesNotExistAsync(copyFileRequest.DestinationFilePath, cancellationToken).ConfigureAwait(false);
            
            await CopyFileInternalAsync(
                copyFileRequest.SourceFilePath,
                copyFileRequest.DestinationFilePath,
                cancellationToken
            ).ConfigureAwait(false);

            return new FileRepresentation {Path = copyFileRequest.DestinationFilePath};
        }

        /// <inheritdoc />
        public async Task DeleteFileAsync(DeleteFileRequest deleteFileRequest, CancellationToken cancellationToken)
        {
            await EnsureFileExistsAsync(deleteFileRequest.FilePath, cancellationToken).ConfigureAwait(false);
            await DeleteFileInternalAsync(deleteFileRequest.FilePath, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> FileExistsAsync(
            FileExistsRequest fileExistsRequest,
            CancellationToken cancellationToken
        )
        {
            return await FileExistsInternalAsync(fileExistsRequest.FilePath, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<FileRepresentation> GetFileAsync(
            GetFileRequest getFileRequest, 
            CancellationToken cancellationToken
        )
        {
            var fileExists = await FileExistsInternalAsync(getFileRequest.FilePath, cancellationToken).ConfigureAwait(false);
            return fileExists ? new FileRepresentation {Path = getFileRequest.FilePath} : null;
        }

        /// <inheritdoc />
        public async Task<FileRepresentation> MoveFileAsync(
            MoveFileRequest moveFileRequest, 
            CancellationToken cancellationToken
        )
        {
            await EnsureFileExistsAsync(moveFileRequest.SourceFilePath, cancellationToken).ConfigureAwait(false);
            await EnsureFileDoesNotExistAsync(moveFileRequest.DestinationFilePath, cancellationToken).ConfigureAwait(false);

            await CopyFileInternalAsync(
                moveFileRequest.SourceFilePath,
                moveFileRequest.DestinationFilePath,
                cancellationToken
            ).ConfigureAwait(false);

            await DeleteFileInternalAsync(moveFileRequest.SourceFilePath, cancellationToken).ConfigureAwait(false);

            return new FileRepresentation { Path = moveFileRequest.DestinationFilePath };
        }

        /// <inheritdoc />
        public async Task<string> ReadFileAsStringAsync(
            ReadFileAsStringRequest readFileAsStringRequest,
            CancellationToken cancellationToken
        )
        {
            await EnsureFileExistsAsync(readFileAsStringRequest.FilePath, cancellationToken).ConfigureAwait(false);

            var file = await _s3Client.GetObjectAsync(
                _adapterConfiguration.BucketName,
                readFileAsStringRequest.FilePath.NormalisedPath,
                cancellationToken
            ).ConfigureAwait(false);

            return await new StreamReader(file.ResponseStream).ReadToEndAsync().ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<FileRepresentation> TouchFileAsync(
            TouchFileRequest touchFileRequest,
            CancellationToken cancellationToken
        )
        {
            await EnsureFileDoesNotExistAsync(touchFileRequest.FilePath, cancellationToken).ConfigureAwait(false);
            await TouchFileInternalAsync(touchFileRequest.FilePath, cancellationToken).ConfigureAwait(false);
            return new FileRepresentation { Path = touchFileRequest.FilePath };
        }

        /// <inheritdoc />
        public async Task WriteTextToFileAsync(
            WriteTextToFileRequest writeTextToFileRequest,
            CancellationToken cancellationToken
        )
        {
            await _s3Client.PutObjectAsync(
                new PutObjectRequest
                {
                    BucketName = _adapterConfiguration.BucketName,
                    ContentBody = writeTextToFileRequest.TextToWrite,
                    ContentType = writeTextToFileRequest.ContentType,
                    Key = writeTextToFileRequest.FilePath.NormalisedPath
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Checks and returns whether a file exists. For use within methods that do their own validation.
        /// </summary>
        /// <param name="path">The path to check to see if it exists.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>Whether the file exists or not.</returns>
        private async Task<bool> FileExistsInternalAsync(PathRepresentation path, CancellationToken cancellationToken)
        {
            try
            {
                await _s3Client.GetObjectMetadataAsync(
                    _adapterConfiguration.BucketName,
                    path.NormalisedPath,
                    cancellationToken
                ).ConfigureAwait(false);

                return true;
            }
            catch (Exception e)
            {
                if (e is AmazonS3Exception s3Exception && s3Exception.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// Copies a file without performing any validation. For use within methods that do their own validation.
        /// </summary>
        /// <param name="source">The file to copy.</param>
        /// <param name="destination">The destination to copy it to.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        private async Task CopyFileInternalAsync(
            PathRepresentation source,
            PathRepresentation destination,
            CancellationToken cancellationToken
        )
        {
            await _s3Client.CopyObjectAsync(
                _adapterConfiguration.BucketName,
                source.NormalisedPath,
                _adapterConfiguration.BucketName,
                destination.NormalisedPath,
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a file without performing any validation. For use within methods that do their own validation.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        private async Task DeleteFileInternalAsync(PathRepresentation file, CancellationToken cancellationToken)
        {
            await _s3Client.DeleteObjectAsync(
                _adapterConfiguration.BucketName,
                file.NormalisedPath,
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Touches (i.e. creates a blank file) without performing any validation. For use within methods that do their
        /// own validation.
        /// </summary>
        /// <param name="file">The file to create.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        private async Task TouchFileInternalAsync(PathRepresentation file, CancellationToken cancellationToken)
        {
            await _s3Client.PutObjectAsync(
                new PutObjectRequest
                {
                    BucketName = _adapterConfiguration.BucketName,
                    ContentBody = "",
                    ContentType = "text/plain",
                    Key = file.NormalisedPath
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Used only in methods inside of this class, EnsureFileDoesNotExistAsync does the opposite of
        /// <see cref="EnsureFileExistsAsync" /> and verifies that the file DOES NOT exist, or it throws an exception.
        /// </summary>
        /// <param name="path">The path to the file that should not exist.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task EnsureFileDoesNotExistAsync(PathRepresentation path, CancellationToken cancellationToken)
        {
            if (await FileExistsInternalAsync(path, cancellationToken).ConfigureAwait(false))
            {
                throw new FileAlreadyExistsException(path.NormalisedPath);
            }
        }

        /// <summary>
        /// Used only in methods inside of this class, EnsureFileExistsAsync checks if the requested file exists and, if
        /// it doesn't, throws a <see cref="FileNotFoundException"/>.
        /// </summary>
        /// <param name="path">The path to check exists.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task EnsureFileExistsAsync(PathRepresentation path, CancellationToken cancellationToken)
        {
            if (!await FileExistsInternalAsync(path, cancellationToken).ConfigureAwait(false))
            {
                throw new FileNotFoundException(path.NormalisedPath);
            }
        }
        
        /// <summary>
        /// Lists all of the files under a particular path prefix within S3.
        /// </summary>
        /// <param name="path">The path to use as a prefix.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <param name="marker">
        /// A point at which the listing should continue. Useful for paginating large directories as S3 is limited
        /// to 1000 objects returned from the list endpoint.
        /// </param>
        /// <returns>The response from S3 containing the files (if there are any) for the prefix.</returns>
        private async Task<ListObjectsResponse> ListFilesUnderPath(
            PathRepresentation path, 
            CancellationToken cancellationToken,
            string marker = null
        )
        {
            return await _s3Client.ListObjectsAsync(
                new ListObjectsRequest
                {
                    BucketName = _adapterConfiguration.BucketName,
                    Prefix = path.S3SafeDirectoryPath(),
                    Marker = marker
                },
                cancellationToken
            ).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Retrieves all of the files under a particular path prefix and performs an action until actions are performed
        /// on each page within the paginated result set.
        /// </summary>
        /// <param name="path">The path prefix to retrieve files under.</param>
        /// <param name="action">The action to perform on the returned response.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The S3 objects listed under the path for any bulk remedial actions.</returns>
        private async Task<List<S3Object>> ListPaginatedFilesUnderPathAndPerformActionUntilCompleteAsync(
            PathRepresentation path,
            Func<ListObjectsResponse, Task> action,
            CancellationToken cancellationToken
        )
        {
            string marker = null;
            var objects = new List<S3Object>();
            
            ListObjectsResponse objectsInPrefix;
            do
            {
                objectsInPrefix = await ListFilesUnderPath(path, cancellationToken, marker).ConfigureAwait(false);
                
                if (objectsInPrefix.S3Objects == null || !objectsInPrefix.S3Objects.Any())
                {
                    break;
                }

                marker = objectsInPrefix.NextMarker;
                objects.AddRange(objectsInPrefix.S3Objects);

                await action(objectsInPrefix).ConfigureAwait(false);
            } while (objectsInPrefix.IsTruncated);

            return objects;
        }
    }
}
