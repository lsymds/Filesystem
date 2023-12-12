namespace LSymds.Filesystem;

/// <summary>
/// Representation of an adapter agnostic file.
/// </summary>
public record FileRepresentation
{
    /// <summary>
    /// Gets or sets the file's path information.
    /// </summary>
    public PathRepresentation Path { get; init; }
}
