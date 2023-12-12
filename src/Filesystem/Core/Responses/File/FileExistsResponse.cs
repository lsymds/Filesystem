namespace LSymds.Filesystem;

/// <summary>
/// Response returned from the FileManager.Exists method.
/// </summary>
public record FileExistsResponse
{
    /// <summary>
    /// Gets or sets whether the file does exist or not.
    /// </summary>
    public bool FileExists { get; init; }
}
