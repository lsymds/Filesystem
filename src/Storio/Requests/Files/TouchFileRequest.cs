namespace Storio
{
    /// <summary>
    /// Request used to touch (create without content) a file.
    /// </summary>
    public class TouchFileRequest
    {
        /// <summary>
        /// Gets or sets the path to touch.
        /// </summary>
        public PathRepresentation PathToTouch { get; set; }
    }
}
