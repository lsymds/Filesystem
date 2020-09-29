using System;

namespace Storio.Internal.Validators
{
    /// <summary>
    /// Validation methods for the <see cref="BaseFileRequest" /> class.
    /// </summary>
    public class BaseFileRequestValidator
    {
        /// <summary>
        /// Validates the base file request class by ensuring it contains information that is required regardless of
        /// what the super request is.
        /// </summary>
        /// <param name="request">The base file request.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="PathIsADirectoryException"></exception>
        public static void ValidateAndThrowIfUnsuccessful(BaseFileRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            
            if (request.FilePath == null)
                throw new ArgumentNullException(nameof(request.FilePath));
            
            if (request.FilePath.FinalPathPartIsObviouslyADirectory)
                throw new PathIsADirectoryException(request.FilePath.OriginalPath);
        }
    }
}
