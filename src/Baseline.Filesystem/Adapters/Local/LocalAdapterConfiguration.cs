using System;

namespace Baseline.Filesystem;

/// <summary>
/// Configuration object for the <see cref="LocalAdapter"/> adapter.
/// </summary>
/// <param name="GetPublicUrlForPath">
/// Gets a delegate that resolves a path representation to a publicly accessible URL. This is useful where another
/// process serves a given directory to the world.
/// </param>
public record LocalAdapterConfiguration(Func<PathRepresentation, string> GetPublicUrlForPath);
