using System;
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
        public Task<bool> FileExistsAsync(FileExistsRequest fileExistsRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
        public Task<FileRepresentation> TouchFileAsync(TouchFileRequest touchFileRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task WriteTextToFileAsync(WriteTextToFileRequest writeTextToFileRequest, CancellationToken cancellationToken)
        {
            var response = await _s3Client.PutObjectAsync(
                new PutObjectRequest
                {
                    BucketName = _adapterConfiguration.BucketName,
                    ContentBody = writeTextToFileRequest.TextToWrite,
                    ContentType = writeTextToFileRequest.ContentType,
                    Key = CombineRootAndRequestedPath(writeTextToFileRequest.FilePath).NormalisedPath
                },
                cancellationToken
            );
            
            ValidateBasicResponse(response);
        }

        /// <summary>
        /// Validates the base response that is received from all S3 client calls.
        /// </summary>
        /// <param name="response">The response to validate.</param>
        private static void ValidateBasicResponse(AmazonWebServiceResponse response)
        {
            if (!response.HttpStatusCode.IsSuccessStatusCode())
            {
                throw new WriteTextToFileException(
                    $"HttpStatusCode response from AWS S3 was not successful (received {response.HttpStatusCode})."
                );
            }
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
