using System.IO;

namespace Baseline.Filesystem;

/// <summary>
/// An representation of a file that is persisted in memory.
/// </summary>
/// <param name="ContentType">Gets the content type of the file.</param>
/// <param name="Content">Gets the content of the file.</param>
public record MemoryFileRepresentation(string ContentType, Stream Content);
