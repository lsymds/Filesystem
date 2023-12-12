namespace LSymds.Filesystem;

/// <summary>
/// Response returned from the DirectoryManager.Move method.
/// </summary>
public record MoveDirectoryResponse
{
    /// <summary>
    /// Gets or sets the destination directory representation, i.e. where the source directory was moved to.
    /// </summary>
    public DirectoryRepresentation DestinationDirectory { get; init; }
}
