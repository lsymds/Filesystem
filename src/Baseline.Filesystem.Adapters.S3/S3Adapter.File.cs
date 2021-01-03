using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
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
            await CheckFileExistsAsync(copyFileRequest.SourceFilePath, cancellationToken);
            await CheckFileDoesNotExistAsync(copyFileRequest.DestinationFilePath, cancellationToken);

            await CatchAndWrapProviderExceptions(
                () => _s3Client.CopyObjectAsync(
                    _adapterConfiguration.BucketName,
                    CombineRootAndRequestedPath(copyFileRequest.SourceFilePath).NormalisedPath,
                    _adapterConfiguration.BucketName,
                    CombineRootAndRequestedPath(copyFileRequest.DestinationFilePath).NormalisedPath,
                    cancellationToken
                )
            );

            return new FileRepresentation {Path = copyFileRequest.DestinationFilePath};
        }

        /// <inheritdoc />
        public async Task DeleteFileAsync(DeleteFileRequest deleteFileRequest, CancellationToken cancellationToken)
        {
            await CheckFileExistsAsync(deleteFileRequest.FilePath, cancellationToken);

            await CatchAndWrapProviderExceptions(
                () => _s3Client.DeleteObjectAsync(
                    _adapterConfiguration.BucketName,
                    CombineRootAndRequestedPath(deleteFileRequest.FilePath).NormalisedPath,
                    cancellationToken
                )
            );
        }

        /// <inheritdoc />
        public async Task<bool> FileExistsAsync(
            FileExistsRequest fileExistsRequest,
            CancellationToken cancellationToken
        )
        {
            try
            {
                await _s3Client
                    .GetObjectMetadataAsync(
                        _adapterConfiguration.BucketName,
                        CombineRootAndRequestedPath(fileExistsRequest.FilePath).NormalisedPath,
                        cancellationToken
                    )
                    .ConfigureAwait(false);

                return true;
            }
            catch (Exception e)
            {
                if (e is AmazonS3Exception s3Exception && s3Exception.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                throw ExceptionForS3Exception(e);
            }
        }

        /// <inheritdoc />
        public async Task<FileRepresentation> GetFileAsync(GetFileRequest getFileRequest, CancellationToken cancellationToken)
        {
            var fileExists = await FileExistsAsync(
                new FileExistsRequest { FilePath = getFileRequest.FilePath },
                cancellationToken
            );
            
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
            );

            await DeleteFileAsync(
                new DeleteFileRequest {FilePath = moveFileRequest.SourceFilePath},
                cancellationToken
            );

            return copiedFileResult;
        }

        /// <inheritdoc />
        public async Task<string> ReadFileAsStringAsync(
            ReadFileAsStringRequest readFileAsStringRequest,
            CancellationToken cancellationToken
        )
        {
            await CheckFileExistsAsync(readFileAsStringRequest.FilePath, cancellationToken);

            var file = await CatchAndWrapProviderExceptions(
                () => _s3Client.GetObjectAsync(
                    _adapterConfiguration.BucketName,
                    CombineRootAndRequestedPath(readFileAsStringRequest.FilePath).NormalisedPath,
                    cancellationToken
                )
            );
            return await new StreamReader(file.ResponseStream).ReadToEndAsync();
        }

        /// <inheritdoc />
        public async Task<FileRepresentation> TouchFileAsync(
            TouchFileRequest touchFileRequest,
            CancellationToken cancellationToken
        )
        {
            await CatchAndWrapProviderExceptions(
                () => _s3Client.PutObjectAsync(
                    new PutObjectRequest
                    {
                        BucketName = _adapterConfiguration.BucketName,
                        ContentBody = "",
                        ContentType = "text/plain",
                        Key = CombineRootAndRequestedPath(touchFileRequest.FilePath).NormalisedPath
                    },
                    cancellationToken
                )
            );

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
            await CatchAndWrapProviderExceptions(
                () => _s3Client.PutObjectAsync(
                    new PutObjectRequest
                    {
                        BucketName = _adapterConfiguration.BucketName,
                        ContentBody = writeTextToFileRequest.TextToWrite,
                        ContentType = writeTextToFileRequest.ContentType,
                        Key = CombineRootAndRequestedPath(writeTextToFileRequest.FilePath).NormalisedPath
                    },
                    cancellationToken
                )
            );
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
            var fileExists = await FileExistsAsync(new FileExistsRequest {FilePath = path}, cancellationToken);
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
            var fileExists = await FileExistsAsync(new FileExistsRequest {FilePath = path}, cancellationToken);
            if (!fileExists)
            {
                throw new FileNotFoundException(
                    CombineRootAndRequestedPath(path).NormalisedPath,
                    path.NormalisedPath
                );
            }
        }
    }
}
