namespace Baseline.Filesystem
{
    /// <summary>
    /// Base class for requests that operate against a source and a destination (for example the copy and move
    /// operations).
    /// </summary>
    public abstract class BaseSourceAndDestinationFileRequest<T> : BaseRequest<T>
        where T : BaseSourceAndDestinationFileRequest<T>, new()
    {
        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        public PathRepresentation SourceFilePath { get; set; }
        
        /// <summary>
        /// Gets or sets the destination path.
        /// </summary>
        public PathRepresentation DestinationFilePath { get; set; }

        /// <summary>
        /// Combines the paths belonging to this request with a root path, if the root path is not null.
        /// </summary>
        /// <param name="rootPath">The root path to combine the current paths with.</param>
        /// <returns>
        /// A cloned version of the current class, with the paths combined with the root path if applicable.
        /// </returns>
        internal override T CloneAndCombinePathsWithRootPath(PathRepresentation rootPath)
        {
            if (rootPath == null)
            {
                return (T)this;
            }

            var cloned = ShallowClone();
            cloned.SourceFilePath = cloned.SourceFilePath.CombineWithBase(rootPath);
            cloned.DestinationFilePath = cloned.DestinationFilePath.CombineWithBase(rootPath);
            return cloned;
        }
        
        /// <summary>
        /// Clones the current instance for path updates. Some properties do not need to be cloned (for example path
        /// representations) as they're never modified.
        /// </summary>
        /// <returns>A clone of the current instance.</returns>
        internal override T ShallowClone()
        {
            return new T
            {
                SourceFilePath = SourceFilePath,
                DestinationFilePath = DestinationFilePath
            };
        }
    }
}
