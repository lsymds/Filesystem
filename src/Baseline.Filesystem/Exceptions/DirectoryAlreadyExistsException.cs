namespace Baseline.Filesystem
{
    /// <summary>
    /// Thrown when an attempt is made to create, copy or move a directory to somewhere that already exists.
    /// </summary>
    public class DirectoryAlreadyExistsException : BaselineFilesystemException
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="DirectoryAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="pathWithRoot">The requested path combined with the root path of the adapter if set.</param>
        /// <param name="path">The requested path without the root path of the adapter.</param>
        public DirectoryAlreadyExistsException(string pathWithRoot, string path)
            : base($"The directory (path with root: {pathWithRoot}, path without root: {path}) already exists " +
                   $"and cannot be created, copied to or moved to.")
        {
        }
    }
}
