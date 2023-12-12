namespace LSymds.Filesystem;

/// <summary>
/// Response returned from the FileManager.ReadAsString method.
/// </summary>
public record ReadFileAsStringResponse
{
    /// <summary>
    /// Gets or sets the file's contents in string format.
    /// </summary>
    public string FileContents { get; init; }
}
