namespace Baseline.Filesystem
{
    /// <summary>
    /// Response representing the outcome of action to copy a file.
    /// </summary>
    public class CopyFileResponse
    {
        /// <summary>
        /// Gets or sets the file information of the destination file.
        /// </summary>
        public FileRepresentation DestinationFile { get; set; }
    }
}
