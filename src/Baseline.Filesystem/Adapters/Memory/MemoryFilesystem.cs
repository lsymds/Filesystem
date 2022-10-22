using System.Collections.Generic;
using Baseline.Filesystem.Internal.Adapters.Memory;

namespace Baseline.Filesystem;

/// <summary>
/// An in-memory representation of a filesystem, with helper methods to make understanding it easier.
/// </summary>
public class MemoryFilesystem
{
    /// <summary>
    /// Gets the root directory which is the base of the memory filesystem.
    /// </summary>
    public MemoryDirectoryRepresentation RootDirectory { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryFilesystem"/> class.
    /// </summary>
    public MemoryFilesystem()
    {
        RootDirectory = new(
            Path: "/".AsBaselineFilesystemPath(),
            ChildDirectories: new Dictionary<PathRepresentation, MemoryDirectoryRepresentation>(),
            Files: new List<MemoryFileRepresentation>()
        );
    }

    /// <summary>
    /// Ensures that the given directory path representation exists within memory.
    /// </summary>
    public bool DirectoryExists(PathRepresentation path)
    {
        if (path.NormalisedPath == "")
        {
            return true;
        }

        var workingDirectory = RootDirectory;

        foreach (var pathPart in path.GetPathTree())
        {
            if (!workingDirectory.ChildDirectories.ContainsKey(pathPart))
            {
                return false;
            }
        }

        return true;
    }
}
