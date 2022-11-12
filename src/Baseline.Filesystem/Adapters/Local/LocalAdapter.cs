using System;

namespace Baseline.Filesystem;

/// <summary>
/// An <see cref="IAdapter"/> implementation for interacting with files and directories on a local disk (or one
/// masquerading as one).
/// </summary>
public partial class LocalAdapter : IAdapter
{
    private readonly LocalAdapterConfiguration _configuration;

    /// <summary>
    /// Initialises a new instance of the <see cref="LocalAdapter"/> class.
    /// </summary>
    public LocalAdapter(LocalAdapterConfiguration configuration)
    {
        if (configuration.GetPublicUrlForPath is null)
        {
            throw new ArgumentNullException(
                nameof(configuration.GetPublicUrlForPath),
                "A delegate that retrieves public URLs to return for a given path is required. It can be anything, we just don't want to make any assumptions for you!"
            );
        }

        _configuration = configuration;
    }
}
