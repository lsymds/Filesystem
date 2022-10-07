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
        /// <param name="path">The requested path that already exists.</param>
        public DirectoryAlreadyExistsException(string path)
            : base(
                $"The directory ({path}) already exists and cannot be created, copied to or moved to."
            ) { }
    }
}
