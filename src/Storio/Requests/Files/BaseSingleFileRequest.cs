namespace Storio
{
    /// <summary>
    /// Base class containing all properties for basic file requests that only operate against one file.
    /// </summary>
    public abstract class BaseSingleFileRequest
    {
        /// <summary>
        /// Gets or sets the file path to use to perform the action against.
        /// </summary>
        public PathRepresentation FilePath { get; set; }
    }
}
