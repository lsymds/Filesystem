using System;

namespace LSymds.Filesystem;

/// <summary>
/// Configuration object for the <see cref="LocalAdapter"/> adapter.
/// </summary>
public record LocalAdapterConfiguration
{
    /// <summary>
    /// Gets or sets a delegate that resolves a path representation to a publicly accessible URL. This is useful where
    /// another process serves a given directory to the world.
    /// </summary>
    public Func<PathRepresentation, string> GetPublicUrlForPath { get; set; }
}
