using System;

namespace Baseline.Filesystem.Internal.Validators.Files;

/// <summary>
/// Validators for the <see cref="GetFilePublicUrlRequest"/> class.
/// </summary>
internal class GetFilePublicUrlRequestValidator
{
    /// <summary>
    /// Validates the <see cref="GetFilePublicUrlRequest"/> request class and throws any applicable exceptions.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    public static void ValidateAndThrowIfUnsuccessful(GetFilePublicUrlRequest request)
    {
        BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(request);

        if (request.Expiry != null && request.Expiry <= DateTime.Now.AddSeconds(10))
        {
            throw new ArgumentException(
                "Expiry cannot be less than 10 seconds away from the current time.",
                nameof(request.Expiry)
            );
        }
    }
}
