namespace Baseline.Filesystem
{
    /// <summary>
    /// Representation of an adapter aware file.
    /// </summary>
    public class AdapterAwareFileRepresentation : AdapterAwareRepresentation
    {
        /// <summary>
        /// Gets or sets the file representation.
        /// </summary>
        public FileRepresentation File { get; set; }
    }
}
