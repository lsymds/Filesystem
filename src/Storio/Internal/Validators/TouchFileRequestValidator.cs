using System;

namespace Storio.Internal.Validators
{
    /// <summary>
    /// Validation methods for the <see cref="TouchFileRequest" /> class.
    /// </summary>
    internal static class TouchFileRequestValidator
    {
        /// <summary>
        /// Validates the request object according to a set of criteria and throws exceptions if any of those criteria
        /// fail.
        /// </summary>
        /// <param name="request">The request to check.</param>
        /// <exception cref="ArgumentNullException" />
        public static void ValidateAndThrowIfUnsuccessful(TouchFileRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            
            if (request.PathToTouch == null)
                throw new ArgumentNullException(nameof(request.PathToTouch));
            
            if (request.PathToTouch.FinalPathPartIsObviouslyADirectory)
                throw new PathIsADirectoryException(request.PathToTouch.OriginalPath);
        }
    }
}
