using System;

namespace Baseline.Filesystem;

/// <summary>
/// Configuration options for the memory adapter.
/// </summary>
public record MemoryAdapterConfiguration
{
    /// <summary>
    /// Gets the TTL (time-to-live) that should be used for each resource within the memory adapter. By default, this is
    /// set to TimeSpan.MaxValue (near enough forever).
    /// </summary>
    public TimeSpan TimeToLive { get; init; } = TimeSpan.MaxValue;

    /// <summary>
    /// Gets the class that represents the in memory filesystem. Chances are you won't ever need to override this, but
    /// if you do, you'll be glad that it's here.
    /// </summary>
    public MemoryFilesystem MemoryFilesystem { get; init; } = new();
}
