using System;

namespace Baseline.Filesystem.Internal.Validators.Files
{
    /// <summary>
    /// Validation methods for the <see cref="WriteStreamToFileRequest"/> class.
    /// </summary>
    internal static class WriteStreamToFileRequestValidator
    {
        /// <summary>
        /// Validates a <see cref="WriteStreamToFileRequest"/> ensuring it is safe to pass to the rest of the library.
        /// Throws an exception if it does not validate successfully.
        /// </summary>
        /// <param name="writeStreamToFileRequest">The request to validate.</param>
        public static void ValidateAndThrowIfUnsuccessful(WriteStreamToFileRequest writeStreamToFileRequest)
        {
            BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(writeStreamToFileRequest);

            if (writeStreamToFileRequest.Stream == null)
            {
                throw new ArgumentNullException(nameof(writeStreamToFileRequest.Stream));
            }

            if (!writeStreamToFileRequest.Stream.CanRead)
            {
                throw new ArgumentException("Provided stream is not readable.", nameof(writeStreamToFileRequest.Stream));
            }
        }
    }
}
