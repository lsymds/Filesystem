using System.Collections.Generic;

namespace Baseline.Filesystem.Internal.Adapters.Memory;

/// <summary>
/// A representation of an in-memory directory.
/// </summary>
/// <param name="Path">Gets the path to the directory.</param>
/// <param name="ChildDirectories">Gets any child directories (one level deep) that sit in this directory.</param>
/// <param name="Files">Gets any files within this directory.</param>
public record MemoryDirectoryRepresentation(
    PathRepresentation Path,
    Dictionary<PathRepresentation, MemoryDirectoryRepresentation> ChildDirectories,
    List<MemoryFileRepresentation> Files
);
