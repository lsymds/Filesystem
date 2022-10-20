using System;

namespace Baseline.Filesystem.Internal.Validators.Directories;

/// <summary>
/// Validation methods for the <see cref="IterateDirectoryContentsRequest"/> class.
/// </summary>
internal static class IterateDirectoryContentsRequestValidator
{
    /// <summary>
    /// Validates the request and throws any exceptions if that validation is unsuccessful.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    public static void ValidateAndThrowIfUnsuccessful(IterateDirectoryContentsRequest request)
    {
        BaseSingleDirectoryRequestValidator.ValidateAndThrowIfUnsuccessful(request);

        if (request.Action == null)
        {
            throw new ArgumentNullException(nameof(request.Action));
        }
    }
}
