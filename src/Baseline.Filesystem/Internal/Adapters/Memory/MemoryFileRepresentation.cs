namespace Baseline.Filesystem.Internal.Adapters.Memory;

/// <summary>
/// An representation of a file that is persisted in memory.
/// </summary>
/// <param name="Path">Gets the path to the file.</param>
/// <param name="ContentType">Gets the content type of the file.</param>
/// <param name="Content">Gets the content of the file.</param>
public record MemoryFileRepresentation(PathRepresentation Path, string ContentType, string Content);
