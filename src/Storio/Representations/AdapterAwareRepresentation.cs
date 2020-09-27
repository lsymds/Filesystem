namespace Storio
{
    /// <summary>
    /// Base class used to make previously adapter ignorant classes adapter aware.
    /// </summary>
    public abstract class AdapterAwareRepresentation
    {
        /// <summary>
        /// Gets the name of the adapter.
        /// </summary>
        public string AdapterName { get; internal set; }
    }
}
