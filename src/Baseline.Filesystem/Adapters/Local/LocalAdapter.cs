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
        _configuration = configuration;
    }
}
