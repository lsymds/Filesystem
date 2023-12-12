namespace LSymds.Filesystem;

/// <summary>
/// Thrown when an operation that requires a file to be present is performed on a file that isn't present.
/// </summary>
public class FileNotFoundException : FilesystemException
{
    /// <summary>
    /// Initialises a new instance of the <see cref="FileNotFoundException" /> class.
    /// </summary>
    /// <param name="path">The path to the file that was not found.</param>
    public FileNotFoundException(string path) : base($"The file ({path}) was not found.") { }
}
