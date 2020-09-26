namespace Storio
{
    /// <summary>
    /// Provides a way to manage files within a number of registered adapters.
    /// </summary>
    public class FileManager : IFileManager
    {
        private readonly IAdapterManager _adapterManager;

        public FileManager(IAdapterManager adapterManager)
        {
            _adapterManager = adapterManager;
        }
    }
}
