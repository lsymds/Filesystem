namespace LSymds.Filesystem;

/// <summary>
/// Response returned from the DirectoryManager.Create method.
/// </summary>
public record CreateDirectoryResponse
{
    /// <summary>
    /// Gets or sets the representation of the directory that was created as part of the request.
    /// </summary>
    public DirectoryRepresentation Directory { get; init; }
}
