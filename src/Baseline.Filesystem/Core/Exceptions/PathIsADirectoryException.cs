namespace Baseline.Filesystem;

/// <summary>
/// Thrown when an attempt is made to perform a file operation on a path that was obviously intended to be
/// a directory (i.e. it had a terminating slash at the end).
///
/// This prevents us from making any assumptions about the consuming applications desires and makes them
/// make the decision for us.
/// </summary>
public class PathIsADirectoryException : InvalidPathException
{
    /// <summary>
    /// Initialises a new instance of the <see cref="PathIsADirectoryException" /> class with reference to the path
    /// the attempt was made on.
    /// </summary>
    /// <param name="path">The path that a file operation was attempted to be performed on.</param>
    public PathIsADirectoryException(string path)
        : base(
            path,
            "An attempt was made to perform a file operation on a path that was obviously intended to "
            + "be a directory (i.e. it had a terminating slash at the end)."
        ) { }
}
