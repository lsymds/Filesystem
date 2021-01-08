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
using Baseline.Filesystem.Internal.Contracts;

namespace Baseline.Filesystem.Adapters.S3
{
    /// <summary>
    /// Provides the file based functions of an <see cref="IAdapter"/> for Amazon's Simple Storage Service. 
    /// </summary>
    public partial class S3Adapter
    {
        /// <inheritdoc />
        public async Task<FileRepresentation> CopyFileAsync(CopyFileRequest copyFileRequest, CancellationToken cancellationToken)
        {
            await CheckFileExistsAsync(copyFileRequest.SourceFilePath, cancellationToken).ConfigureAwait(false);
            await CheckFileDoesNotExistAsync(copyFileRequest.DestinationFilePath, cancellationToken).ConfigureAwait(false);

            await _s3Client.CopyObjectAsync(
                _adapterConfiguration.BucketName,
                CombineRootAndRequestedPath(copyFileRequest.SourceFilePath).NormalisedPath,
                _adapterConfiguration.BucketName,
                CombineRootAndRequestedPath(copyFileRequest.DestinationFilePath).NormalisedPath,
                cancellationToken
            ).ConfigureAwait(false);

            return new FileRepresentation {Path = copyFileRequest.DestinationFilePath};
        }

        /// <inheritdoc />
        public async Task DeleteFileAsync(DeleteFileRequest deleteFileRequest, CancellationToken cancellationToken)
        {
            await CheckFileExistsAsync(deleteFileRequest.FilePath, cancellationToken).ConfigureAwait(false);

            await _s3Client.DeleteObjectAsync(
                _adapterConfiguration.BucketName,
                CombineRootAndRequestedPath(deleteFileRequest.FilePath).NormalisedPath,
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> FileExistsAsync(
            FileExistsRequest fileExistsRequest,
            CancellationToken cancellationToken
        )
        {
            try
            {
                await _s3Client.GetObjectMetadataAsync(
                    _adapterConfiguration.BucketName,
                    CombineRootAndRequestedPath(fileExistsRequest.FilePath).NormalisedPath,
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

        /// <inheritdoc />
        public async Task<FileRepresentation> GetFileAsync(GetFileRequest getFileRequest, CancellationToken cancellationToken)
        {
            var fileExists = await FileExistsAsync(
                new FileExistsRequest { FilePath = getFileRequest.FilePath },
                cancellationToken
            ).ConfigureAwait(false);
            
            return fileExists ? new FileRepresentation {Path = getFileRequest.FilePath} : null;
        }

        /// <inheritdoc />
        public async Task<FileRepresentation> MoveFileAsync(MoveFileRequest moveFileRequest, CancellationToken cancellationToken)
        {
            var copiedFileResult = await CopyFileAsync(
                new CopyFileRequest
                {
                    SourceFilePath = moveFileRequest.SourceFilePath,
                    DestinationFilePath = moveFileRequest.DestinationFilePath
                },
                cancellationToken
            ).ConfigureAwait(false);

            await DeleteFileAsync(
                new DeleteFileRequest {FilePath = moveFileRequest.SourceFilePath},
                cancellationToken
            ).ConfigureAwait(false);

            return copiedFileResult;
        }

        /// <inheritdoc />
        public async Task<string> ReadFileAsStringAsync(
            ReadFileAsStringRequest readFileAsStringRequest,
            CancellationToken cancellationToken
        )
        {
            await CheckFileExistsAsync(readFileAsStringRequest.FilePath, cancellationToken).ConfigureAwait(false);

            var file = await _s3Client.GetObjectAsync(
                _adapterConfiguration.BucketName,
                CombineRootAndRequestedPath(readFileAsStringRequest.FilePath).NormalisedPath,
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
            await _s3Client.PutObjectAsync(
                new PutObjectRequest
                {
                    BucketName = _adapterConfiguration.BucketName,
                    ContentBody = "",
                    ContentType = "text/plain",
                    Key = CombineRootAndRequestedPath(touchFileRequest.FilePath).NormalisedPath
                },
                cancellationToken
            ).ConfigureAwait(false);

            return new FileRepresentation
            {
                Path = touchFileRequest.FilePath
            };
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
                    Key = CombineRootAndRequestedPath(writeTextToFileRequest.FilePath).NormalisedPath
                },
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Used only in methods inside of this class, CheckFileDoesNotExistAsync does the opposite of
        /// <see cref="CheckFileExistsAsync" /> and verifies that the file DOES NOT exist, or it throws an exception.
        /// </summary>
        /// <param name="path">The path to the file that should not exist.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        private async Task CheckFileDoesNotExistAsync(PathRepresentation path, CancellationToken cancellationToken)
        {
            var fileExists = await FileExistsAsync(new FileExistsRequest {FilePath = path}, cancellationToken).ConfigureAwait(false);
            if (fileExists)
            {
                throw new FileAlreadyExistsException(
                    CombineRootAndRequestedPath(path).NormalisedPath,
                    path.NormalisedPath
                );
            }
        }

        /// <summary>
        /// Used only in methods inside of this class, CheckFileExistsAsync checks if the requested file exists and, if
        /// it doesn't, throws a <see cref="FileNotFoundException"/>.
        /// </summary>
        /// <param name="path">The path to check exists.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        private async Task CheckFileExistsAsync(PathRepresentation path, CancellationToken cancellationToken)
        {
            var fileExists = await FileExistsAsync(new FileExistsRequest {FilePath = path}, cancellationToken).ConfigureAwait(false);
            if (!fileExists)
            {
                throw new FileNotFoundException(
                    CombineRootAndRequestedPath(path).NormalisedPath,
                    path.NormalisedPath
                );
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
        private Task<ListObjectsResponse> ListFilesUnderPath(
            PathRepresentation path, 
            CancellationToken cancellationToken,
            string marker = null
        )
        {
            return _s3Client.ListObjectsAsync(
                new ListObjectsRequest
                {
                    BucketName = _adapterConfiguration.BucketName,
                    Prefix = CombineRootAndRequestedPath(path).S3SafeDirectoryPath(),
                    Marker = marker
                },
                cancellationToken
            );
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
                objectsInPrefix = await ListFilesUnderPath(CombineRootAndRequestedPath(path), cancellationToken, marker)
                    .ConfigureAwait(false);
                
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
