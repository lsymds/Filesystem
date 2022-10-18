namespace Baseline.Filesystem;

/// <summary>
/// Response returned from the FileManager.GetFile method.
/// </summary>
public class GetFileResponse
{
    /// <summary>
    /// Gets or sets the file representation that was requested.
    /// </summary>
    public FileRepresentation File { get; set; }
}
