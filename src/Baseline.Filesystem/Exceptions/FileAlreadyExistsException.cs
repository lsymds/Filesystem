namespace Baseline.Filesystem
{
    /// <summary>
    /// Thrown when an operation is performed (or attempted) on a file that already exists.
    /// </summary>
    public class FileAlreadyExistsException : BaselineFilesystemException
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FileAlreadyExistsException" /> class.
        /// </summary>
        /// <param name="pathWithRoot">The path that already exists with the root path (if applicable).</param>
        /// <param name="path">The path to the file excluding the root path.</param>
        public FileAlreadyExistsException(string pathWithRoot, string path)
            : base($"The file (path with root: {pathWithRoot}, path without root: {path}) already exists and cannot " +
                   $"be written to.")
        {}
    }
}
