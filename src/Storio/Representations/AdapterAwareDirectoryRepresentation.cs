namespace Storio
{
    /// <summary>
    /// Representation of an adapter aware directory. 
    /// </summary>
    public class AdapterAwareDirectoryRepresentation : AdapterAwareRepresentation
    {
        /// <summary>
        /// Gets or sets the directory representation that is adapter ignorant.
        /// </summary>
        public DirectoryRepresentation Directory { get; internal set; }
    }
}
