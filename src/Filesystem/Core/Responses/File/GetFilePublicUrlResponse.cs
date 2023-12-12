using System;

namespace LSymds.Filesystem;

/// <summary>
/// Response returned from the GetPublicUrl method that retrieves a publicly accessible URL for the requested
/// file.
/// </summary>
public record GetFilePublicUrlResponse
{
    /// <summary>
    /// Gets or sets the public URL.
    /// </summary>
    public string Url { get; init; }

    /// <summary>
    /// Gets or sets when the public URL expires, if applicable.
    /// </summary>
    public DateTime? Expiry { get; init; }
}
