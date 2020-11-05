using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Storio.Adapters.S3.Internal.Extensions;

namespace Storio.Adapters.S3
{
    /// <summary>
    /// Provides an <see cref="IAdapter"/> implementation for Amazon's Simple Storage Service. 
    /// </summary>
    public class S3Adapter : IAdapter
    {
        private readonly S3AdapterConfiguration _adapterConfiguration;
        private readonly PathRepresentation _basePath;
        private readonly IAmazonS3 _s3Client;

        /// <summary>
        /// Initialises a new instance of the <see cref="S3Adapter"/>, configuring it with options passed in by the
        /// consuming application.
        /// </summary>
        /// <param name="adapterConfiguration">The configuration used to configure this adapter.</param>
        public S3Adapter(S3AdapterConfiguration adapterConfiguration)
        {
            _adapterConfiguration = adapterConfiguration;
            _s3Client = _adapterConfiguration.S3Client;
            
            if (!string.IsNullOrWhiteSpace(adapterConfiguration.RootPath))
            {
                _basePath = new PathRepresentationBuilder(adapterConfiguration.RootPath).Build();   
            }
        }

        /// <inheritdoc />
        public Task<FileRepresentation> CopyFileAsync(CopyFileRequest copyFileRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task DeleteFileAsync(DeleteFileRequest deleteFileRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
                );

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
        public Task<FileRepresentation> GetFileAsync(GetFileRequest getFileRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<FileRepresentation> MoveFileAsync(MoveFileRequest moveFileRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<string> ReadFileAsStringAsync(ReadFileAsStringRequest readFileAsStringRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<FileRepresentation> TouchFileAsync(
            TouchFileRequest touchFileRequest,
            CancellationToken cancellationToken
        )
        {
            await CatchAndWrapProviderExceptions(
                async () => await _s3Client.PutObjectAsync(
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
                async () => await _s3Client.PutObjectAsync(
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
        /// Executes the function that communicates with the adapter's provider and, if an exception is thrown, wraps
        /// it into one that is easily communicable 
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        /// <exception cref="AdapterProviderOperationException"></exception>
        private static async Task<TResponse> CatchAndWrapProviderExceptions<TResponse>(Func<Task<TResponse>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception e)
            {
                throw ExceptionForS3Exception(e);
            }    
        }

        /// <summary>
        /// Gets a <see cref="AdapterProviderOperationException" /> detailing that an unhandled exception occurred
        /// when calling a provider endpoint.
        /// </summary>
        /// <param name="e">The exception to wrap.</param>
        /// <returns>The wrapped exception that should be thrown.</returns>
        private static AdapterProviderOperationException ExceptionForS3Exception(Exception e)
        {
            return new AdapterProviderOperationException(
                "Unexpected exception thrown when communicating with the Amazon S3 endpoint.",
                e
            );
        }

        /// <summary>
        /// Combines the root path (if specified) with the path specified as part of the request.
        /// </summary>
        /// <param name="requestedPath">The path specified as part of the request.</param>
        /// <returns>The combined paths, if applicable, or the request path if not.</returns>
        private PathRepresentation CombineRootAndRequestedPath(PathRepresentation requestedPath)
        {
            if (_basePath == null)
            {
                return requestedPath;
            }
            
            return new PathCombinationBuilder(_basePath, requestedPath).Build();
        }
    }
}
