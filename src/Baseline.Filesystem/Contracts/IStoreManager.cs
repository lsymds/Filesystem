namespace Baseline.Filesystem
{
    /// <summary>
    /// Provides an interface for the management of stores. This is where the initial magic happens as Baseline.Filesystem allows
    /// the registration of multiple stores across many different provider adapters. The other Baseline.Filesystem interfaces and their
    /// implementations (<see cref="IDirectoryManager" /> and <see cref="IFileManager" />) utilise implementations of
    /// this interface to become store aware.
    /// </summary>
    public interface IStoreManager
    {
        /// <summary>
        /// Gets a store registration by its registered name.
        /// </summary>
        /// <param name="name">The name of the store.</param>
        /// <returns>The store registration if it is found.</returns>
        /// <exception cref="StoreNotFoundException" />
        StoreRegistration Get(string name);

        /// <summary>
        /// Registers a store by a specified name. Names are normalised (lowercased). Duplicates will result in an
        /// exception being thrown.
        /// </summary>
        /// <param name="registration">The registration class containing the adapter (i.e. provider), its name, and its root path.</param>
        /// <exception cref="StoreAlreadyRegisteredException" />
        void Register(StoreRegistration registration);
    }
}
