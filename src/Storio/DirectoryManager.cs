namespace Storio
{
    /// <summary>
    /// Provides a way to manage directories within a number of registered adapters.
    /// </summary>
    public class DirectoryManager : IDirectoryManager
    {
        private readonly IAdapterManager _adapterManager;

        public DirectoryManager(IAdapterManager adapterManager)
        {
            _adapterManager = adapterManager;
        }
    }
}
