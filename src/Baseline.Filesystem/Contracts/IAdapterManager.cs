namespace Baseline.Filesystem
{
    /// <summary>
    /// Provides an interface for the management of adapters. This is where the initial magic happens as Baseline.Filesystem allows 
    /// the registration of multiple adapters across many different providers. The other Baseline.Filesystem interfaces and their 
    /// impl-ementations (<see cref="IDirectoryManager" /> and <see cref="IFileManager" />) utilise implementations of
    /// this interface to become adapter aware.
    /// </summary>
    public interface IAdapterManager
    {
        /// <summary>
        /// Gets an adapter by its registered name.
        /// </summary>
        /// <param name="name">The name of the adapter.</param>
        /// <returns>The adapter instance if it is found.</returns>
        /// <exception cref="AdapterNotFoundException" />
        IAdapter Get(string name);

        /// <summary>
        /// Registers an adapter by a specified name. Names are normalised (lowercased). Duplicates will result in an
        /// exception being thrown.
        /// </summary>
        /// <param name="adapter">The adapter to register.</param>
        /// <param name="name">The name of the adapter.</param>
        /// <exception cref="AdapterAlreadyRegisteredException" />
        void Register(IAdapter adapter, string name = "default");
    }
}
