using System;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Exception that is thrown when a directory is not found.
    /// </summary>
    public class DirectoryNotFoundException : BaselineFilesystemException
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="DirectoryNotFoundException"/> class.
        /// </summary>
        /// <param name="pathWithRoot">The requested path combined with the root path of the adapter if set.</param>
        /// <param name="path">The requested path without the root path of the adapter.</param>
        public DirectoryNotFoundException(string pathWithRoot, string path)
            : base($"The directory (path with root: {pathWithRoot}, path without root: {path}) was not found.")
        {
        }
    }
}
