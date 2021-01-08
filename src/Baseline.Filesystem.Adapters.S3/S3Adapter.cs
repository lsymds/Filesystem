using System;
using Amazon.S3;
using Baseline.Filesystem.Internal.Contracts;

namespace Baseline.Filesystem.Adapters.S3
{
    /// <summary>
    /// Provides the shared, directory/file agnostic functions of the <see cref="IAdapter"/> implementation for
    /// Amazon's Simple Storage Service. 
    /// </summary>
    public partial class S3Adapter : IAdapter
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

        /// <summary>
        /// Gets a <see cref="AdapterProviderOperationException" /> detailing that an unhandled exception occurred
        /// when calling a provider endpoint.
        /// </summary>
        /// <param name="e">The exception to wrap.</param>
        /// <returns>The wrapped exception that should be thrown.</returns>
        private static AdapterProviderOperationException ExceptionForS3Exception(Exception e)
        {
            return new AdapterProviderOperationException(
                "Unexpected exception thrown when communicating with the Amazon S3 endpoint. " +
                "See inner exception for details.",
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
