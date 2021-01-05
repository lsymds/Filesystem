using System;

namespace Baseline.Filesystem.Internal.Validators.Files
{
    /// <summary>
    /// Validation methods for the <see cref="BaseSingleFileRequest" /> class.
    /// </summary>
    internal static class BaseSingleFileRequestValidator
    {
        /// <summary>
        /// Validates the base file request class by ensuring it contains information that is required regardless of
        /// what the super request is.
        /// </summary>
        /// <param name="request">The base file request.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="PathIsADirectoryException"></exception>
        public static void ValidateAndThrowIfUnsuccessful(BaseSingleFileRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            FilePathValidator.ValidateAndThrowIfUnsuccessful(request.FilePath);
        }
    }
}
