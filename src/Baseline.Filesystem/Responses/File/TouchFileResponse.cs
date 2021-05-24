namespace Baseline.Filesystem
{
    /// <summary>
    /// A response returned from the FileManager.Touch method.
    /// </summary>
    public class TouchFileResponse
    {
        /// <summary>
        /// Gets or sets the representation of the file that is created.
        /// </summary>
        public FileRepresentation File { get; set; }
    }
}
