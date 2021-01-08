namespace Baseline.Filesystem
{
    /// <summary>
    /// Base class containing all properties for a source and destination based directory request.
    /// </summary>
    public class BaseSourceAndDestinationDirectoryRequest
    {
        /// <summary>
        /// Gets or sets the source directory path.
        /// </summary>
        public PathRepresentation SourceDirectoryPath { get; set; }
        
        /// <summary>
        /// Gets or sets the destination directory path.
        /// </summary>
        public PathRepresentation DestinationDirectoryPath { get; set; }
    }
}
