using System;

namespace Baseline.Filesystem;

/// <summary>
/// Request used to retrieve a public URL for a file.
/// </summary>
public class GetFilePublicUrlRequest : BaseSingleFileRequest<GetFilePublicUrlRequest>
{
    /// <summary>
    /// Gets or sets when the public URL should expire. This is not supported in all adapters, but will be defaulted
    /// if you do not specify it.
    /// </summary>
    public DateTime? Expiry { get; set; }

    /// <inheritdoc />
    internal override GetFilePublicUrlRequest ShallowClone()
    {
        var cloned = base.ShallowClone();
        cloned.Expiry = Expiry;
        return cloned;
    }
}
