using System;

namespace Baseline.Filesystem;

/// <summary>
/// Configuration options for the memory adapter.
/// </summary>
public record MemoryAdapterConfiguration
{
    /// <summary>
    /// Gets or sets the public URL to return should that method be called. The in-memory adapter does not support
    /// returning correct public URLs for files, but this property provides a way not to break everything should
    /// it be called.
    /// </summary>
    public string PublicUrlToReturn { get; init; }

    /// <summary>
    /// Gets the class that represents the in memory filesystem. Chances are you won't ever need to override this, but
    /// if you do, you'll be glad that it's here.
    /// </summary>
    public MemoryFilesystem MemoryFilesystem { get; init; } = new();
}
