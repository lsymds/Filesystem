namespace Baseline.Filesystem
{
    /// <summary>
    /// Base class for requests that operate against a source and a destination (for example the copy and move
    /// operations).
    /// </summary>
    public abstract class BaseSourceAndDestinationFileRequest
    {
        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        public PathRepresentation SourceFilePath { get; set; }
        
        /// <summary>
        /// Gets or sets the destination path.
        /// </summary>
        public PathRepresentation DestinationFilePath { get; set; }
    }
}
