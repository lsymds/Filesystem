using System;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Thrown when an adapter is not found by a specified adapter name.
    /// </summary>
    public class AdapterNotFoundException : BaselineFilesystemException
    {
        /// <summary>
        /// Gets the name of the adapter that could not be found.
        /// </summary>
        public string AdapterName { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="AdapterNotFoundException" /> class referencing the adapter
        /// name that could not be found.
        /// </summary>
        /// <param name="adapterName">The name of the adapter that could not be found.</param>
        public AdapterNotFoundException(string adapterName)
            : base($"The adapter '{adapterName}' was not found. Have you registered it?")
        {
            AdapterName = adapterName;
        }
    }
}
