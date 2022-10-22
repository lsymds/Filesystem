namespace Baseline.Filesystem;

/// <summary>
/// Response returned from the FileManager.Move method.
/// </summary>
public record MoveFileResponse
{
    /// <summary>
    /// Gets or sets the representation of the file in its DESTINATION.
    /// </summary>
    public FileRepresentation DestinationFile { get; init; }
}
