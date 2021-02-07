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
        /// Gets an adapter registration by its registered name.
        /// </summary>
        /// <param name="name">The name of the adapter.</param>
        /// <returns>The adapter registration if it is found.</returns>
        /// <exception cref="AdapterNotFoundException" />
        AdapterRegistration Get(string name);

        /// <summary>
        /// Registers an adapter by a specified name. Names are normalised (lowercased). Duplicates will result in an
        /// exception being thrown.
        /// </summary>
        /// <param name="registration">The registration class containing the adapter, its name, and its root path.</param>
        /// <exception cref="AdapterAlreadyRegisteredException" />
        void Register(AdapterRegistration registration);
    }
}
