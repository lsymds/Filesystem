using System;

namespace Storio
{
    /// <summary>
    /// Thrown when an adapter with the same normalised (lowercased) name has been registered.
    /// </summary>
    public class AdapterAlreadyRegisteredException : Exception
    {
        /// <summary>
        /// The name of the adapter that has previously been registered.
        /// </summary>
        /// <value></value>
        public string AdapterName { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="AdapterAlreadyRegisteredException" /> class by referencing
        /// the duplicate adapter name.
        /// </summary>
        /// <param name="adapterName">The adapter name that is already registered.</param>
        public AdapterAlreadyRegisteredException(string adapterName)
            : base($"The adapter '{adapterName}' is already registered.")
        {
            AdapterName = adapterName;
        }
    }
}
