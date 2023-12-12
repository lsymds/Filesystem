namespace LSymds.Filesystem;

/// <summary>
/// Representation of an adapter agnostic directory.
/// </summary>
public record DirectoryRepresentation
{
    /// <summary>
    /// Gets or sets the the directory's path information.
    /// </summary>
    public PathRepresentation Path { get; init; }
}
