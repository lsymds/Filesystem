namespace Baseline.Filesystem
{
    /// <summary>
    /// Exception that is thrown when a path provided is not obviously a directory, and therefore, in the interest of
    /// preventing any ambiguity, is rejected.
    /// </summary>
    public class PathIsNotObviouslyADirectoryException : InvalidPathException
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="PathIsNotObviouslyADirectoryException"/>.
        /// </summary>
        /// <param name="path">The path which is not obviously a directory.</param>
        public PathIsNotObviouslyADirectoryException(string path)
            : base(
                path,
                "The path specified represents a file or it is ambiguous as to whether it is a "
                    + "directory or not. Suffix the path with a / to denote it is a directory."
            ) { }
    }
}
