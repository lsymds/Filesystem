namespace Baseline.Filesystem
{
    /// <summary>
    /// Base class containing all properties for single directory based requests.
    /// </summary>
    public class BaseSingleDirectoryRequest
    {
        /// <summary>
        /// Gets or sets the directory path to use in the request.
        /// </summary>
        public PathRepresentation DirectoryPath { get; set; }
    }
}
