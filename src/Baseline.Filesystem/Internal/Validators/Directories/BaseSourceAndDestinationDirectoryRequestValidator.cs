using System;

namespace Baseline.Filesystem.Internal.Validators.Directories
{
    /// <summary>
    /// Validation methods for the <see cref="BaseSourceAndDestinationDirectoryRequest{T}"/> class.
    /// </summary>
    public static class BaseSourceAndDestinationDirectoryRequestValidator
    {
        /// <summary>
        /// Validates the request and throws exceptions for any failures.
        /// </summary>
        /// <param name="directoryRequest">The directory request to validate.</param>
        public static void ValidateAndThrowIfUnsuccessful<T>(
            BaseSourceAndDestinationDirectoryRequest<T> directoryRequest
        ) where T : BaseSourceAndDestinationDirectoryRequest<T>, new()
        {
            if (directoryRequest == null)
            {
                throw new ArgumentNullException(nameof(directoryRequest));
            }

            DirectoryPathValidator.ValidateAndThrowIfUnsuccessful(
                directoryRequest.SourceDirectoryPath
            );
            DirectoryPathValidator.ValidateAndThrowIfUnsuccessful(
                directoryRequest.DestinationDirectoryPath
            );
        }
    }
}
