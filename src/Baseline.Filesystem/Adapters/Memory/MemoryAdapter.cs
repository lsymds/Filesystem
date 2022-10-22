namespace Baseline.Filesystem.Memory;

/// <summary>
/// Provides the shared, directory/file agnostic functions of the <see cref="IAdapter"/> implementation within memory.
/// Perfect for tests or systems that need short-lived, ephemeral data stores.
/// </summary>
public partial class MemoryAdapter : IAdapter
{
    private readonly MemoryAdapterConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryAdapter"/> class.
    /// </summary>
    public MemoryAdapter(MemoryAdapterConfiguration configuration)
    {
        _configuration = configuration;
    }
}
