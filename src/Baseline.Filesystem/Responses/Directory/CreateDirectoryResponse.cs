namespace Baseline.Filesystem;

/// <summary>
/// Response returned from the DirectoryManager.Create method.
/// </summary>
public class CreateDirectoryResponse
{
    /// <summary>
    /// Gets or sets the representation of the directory that was created as part of the request.
    /// </summary>
    public DirectoryRepresentation Directory { get; set; }
}
