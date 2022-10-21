using System;

namespace Baseline.Filesystem.Internal.Validators.Directories;

/// <summary>
/// Validation methods for the <see cref="BaseSingleDirectoryRequest{T}"/> class.
/// </summary>
internal static class BaseSingleDirectoryRequestValidator
{
    /// <summary>
    /// Validates the request and throws any exceptions if that validation is unsuccessful.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    public static void ValidateAndThrowIfUnsuccessful<T>(BaseSingleDirectoryRequest<T> request)
        where T : BaseSingleDirectoryRequest<T>, new()
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        DirectoryPathValidator.ValidateAndThrowIfUnsuccessful(request.DirectoryPath);
    }
}
