namespace Baseline.Filesystem
{
    /// <summary>
    /// Thrown when an operation that requires a file to be present is performed on a file that isn't present.
    /// </summary>
    public class FileNotFoundException : BaselineFilesystemException
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="FileNotFoundException" /> class.
        /// </summary>
        /// <param name="pathWithRoot">
        /// The path with the root for the adapter (if one is specified). If one isn't specified this will be the same
        /// as <see cref="path" />.
        /// </param>
        /// <param name="path">The path to the file that was not found.</param>
        public FileNotFoundException(string pathWithRoot, string path)
            : base($"The file (path with root: {pathWithRoot}, path without root: {path}) was not found.")
        {
        }
    }
}
