using System.IO;

namespace Baseline.Filesystem;

/// <summary>
/// Response returned from the FileManager.ReadAsStream method.
/// </summary>
public class ReadFileAsStreamResponse
{
    /// <summary>
    /// Gets or sets the stream of the file contents.
    /// </summary>
    public Stream FileContents { get; set; }
}
