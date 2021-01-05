using System;

namespace Baseline.Filesystem.Internal.Validators.Files
{
    /// <summary>
    /// Validation methods for the <see cref="BaseSourceAndDestinationFileRequest" /> class.
    /// </summary>
    internal static class BaseSourceAndDestinationFileRequestValidator
    {
        /// <summary>
        /// Validates the request and throws exceptions if the validation is unsuccessful.
        /// </summary>
        /// <param name="fileRequest">The request to validate.</param>
        /// <exception cref="ArgumentNullException" />
        public static void ValidateAndThrowIfUnsuccessful(BaseSourceAndDestinationFileRequest fileRequest)
        {
            if (fileRequest == null)
            {
                throw new ArgumentNullException(nameof(fileRequest));
            }

            FilePathValidator.ValidateAndThrowIfUnsuccessful(fileRequest.SourceFilePath);
            FilePathValidator.ValidateAndThrowIfUnsuccessful(fileRequest.DestinationFilePath);
        }
    }
}
