namespace Baseline.Filesystem;

/// <summary>
/// Thrown when an operation is performed (or attempted) on a file that already exists.
/// </summary>
public class FileAlreadyExistsException : BaselineFilesystemException
{
    /// <summary>
    /// Initialises a new instance of the <see cref="FileAlreadyExistsException" /> class.
    /// </summary>
    /// <param name="path">The path to the file that already exists.</param>
    public FileAlreadyExistsException(string path)
        : base($"The file ({path}) already exists and cannot be written to.") { }
}
