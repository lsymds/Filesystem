namespace Baseline.Filesystem;

/// <summary>
/// Response returned from the DirectoryManager.Copy method.
/// </summary>
public record CopyDirectoryResponse
{
    /// <summary>
    /// Gets or sets the representation of the directory that the source directory was copied to.
    /// </summary>
    public DirectoryRepresentation DestinationDirectory { get; init; }
}
