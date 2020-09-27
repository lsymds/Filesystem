namespace Storio
{
    /// <summary>
    /// Provides a way to manage directories within a number of registered adapters.
    /// </summary>
    public class DirectoryManager : IDirectoryManager
    {
        private readonly IAdapterManager _adapterManager;

        /// <summary>
        /// Initialises a new <see cref="DirectoryManager" /> instance with all of its required dependencies.
        /// </summary>
        /// <param name="adapterManager">An adapter manager implementation.</param>
        public DirectoryManager(IAdapterManager adapterManager)
        {
            _adapterManager = adapterManager;
        }
    }
}
