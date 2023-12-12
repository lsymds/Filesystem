namespace LSymds.Filesystem;

/// <summary>
/// Thrown when an attempt is made to use a relative path. As documented, relative paths are not supported as they
/// cannot be easily transferred between adapters.
/// </summary>
public class PathIsRelativeException : InvalidPathException
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PathIsRelativeException" /> class with reference to the
    /// path that caused the error.
    /// </summary>
    /// <param name="path"></param>
    public PathIsRelativeException(string path)
        : base(
            path,
            "Relative paths are not supported in Baseline.Filesystem. Read the documentation to learn more."
        ) { }
}
