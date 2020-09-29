namespace Storio
{
    /// <summary>
    /// Base class containing all properties for basic file requests.
    /// </summary>
    public abstract class BaseFileRequest
    {
        /// <summary>
        /// Gets or sets the file path to use to perform the action against.
        /// </summary>
        public PathRepresentation FilePath { get; set; }
    }
}
