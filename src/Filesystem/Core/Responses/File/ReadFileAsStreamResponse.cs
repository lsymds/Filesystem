using System.IO;

namespace LSymds.Filesystem;

/// <summary>
/// Response returned from the FileManager.ReadAsStream method.
/// </summary>
public record ReadFileAsStreamResponse
{
    /// <summary>
    /// Gets or sets the stream of the file contents.
    /// </summary>
    public Stream FileContents { get; init; }
}
