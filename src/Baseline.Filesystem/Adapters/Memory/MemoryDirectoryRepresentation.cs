using System.Collections.Generic;
using System.Linq;

namespace Baseline.Filesystem;

/// <summary>
/// A representation of an in-memory directory.
/// </summary>
/// <param name="ChildDirectories">Gets any child directories (one level deep) that sit in this directory.</param>
/// <param name="Files">Gets any files within this directory.</param>
public record MemoryDirectoryRepresentation(
    Dictionary<PathRepresentation, MemoryDirectoryRepresentation> ChildDirectories,
    Dictionary<PathRepresentation, MemoryFileRepresentation> Files
)
{
    /// <inheritdoc />
    public override string ToString()
    {
        return $"ChildDirectoriesCount={ChildDirectories.Count} FilesCount={Files.Count}";
    }

    /// <summary>
    /// Deep clones any mutable types within the current <see cref="MemoryDirectoryRepresentation"/> but copies the
    /// references of any immutable types.
    /// </summary>
    public MemoryDirectoryRepresentation DeepCloneMutableTypes()
    {
        MemoryDirectoryRepresentation TraverseAndCloneDirectoryRepresentation(
            MemoryDirectoryRepresentation directoryRepresentation
        )
        {
            var childDirectories =
                new Dictionary<PathRepresentation, MemoryDirectoryRepresentation>();
            var files = directoryRepresentation.Files.ToDictionary(f => f.Key, f => f.Value);

            foreach (var directory in directoryRepresentation.ChildDirectories)
            {
                childDirectories.Add(
                    directory.Key,
                    TraverseAndCloneDirectoryRepresentation(directory.Value)
                );
            }

            // ReSharper disable once WithExpressionModifiesAllMembers
            return this with
            {
                ChildDirectories = childDirectories,
                Files = files
            };
        }

        return TraverseAndCloneDirectoryRepresentation(this);
    }
}
