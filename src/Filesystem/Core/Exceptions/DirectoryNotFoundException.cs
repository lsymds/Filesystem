namespace LSymds.Filesystem;

/// <summary>
/// Exception that is thrown when a directory is not found.
/// </summary>
public class DirectoryNotFoundException : FilesystemException
{
    /// <summary>
    /// Initialises a new instance of the <see cref="DirectoryNotFoundException"/> class.
    /// </summary>
    /// <param name="path">The requested path that does not exist.</param>
    public DirectoryNotFoundException(string path)
        : base($"The directory ({path}) was not found.") { }
}
