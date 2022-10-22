namespace Baseline.Filesystem;

/// <summary>
/// A response returned from the FileManager.Touch method.
/// </summary>
public record TouchFileResponse
{
    /// <summary>
    /// Gets or sets the representation of the file that is created.
    /// </summary>
    public FileRepresentation File { get; init; }
}
