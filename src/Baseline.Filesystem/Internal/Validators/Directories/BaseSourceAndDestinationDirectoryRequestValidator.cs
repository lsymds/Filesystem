using System;

namespace Baseline.Filesystem.Internal.Validators.Directories
{
    /// <summary>
    /// Validation methods for the <see cref="BaseSourceAndDestinationDirectoryRequest"/> class.
    /// </summary>
    public static class BaseSourceAndDestinationDirectoryRequestValidator
    {
        /// <summary>
        /// Validates the request and throws exceptions for any failures.
        /// </summary>
        /// <param name="directoryRequest">The directory request to validate.</param>
        public static void ValidateAndThrowIfUnsuccessful(BaseSourceAndDestinationDirectoryRequest directoryRequest)
        {
            if (directoryRequest == null)
            {
                throw new ArgumentNullException(nameof(directoryRequest));
            }
            
            DirectoryPathValidator.ValidateAndThrowIfUnsuccessful(directoryRequest.SourceDirectoryPath);
            DirectoryPathValidator.ValidateAndThrowIfUnsuccessful(directoryRequest.DestinationDirectoryPath);
        }
    }
}
