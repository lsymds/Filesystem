using Moq;

namespace Baseline.Filesystem.Tests
{
    public abstract class BaseManagerUsageTest
    {
        protected Mock<IAdapter> Adapter { get; set; }
        protected IAdapterManager AdapterManager { get; set; }
        protected IFileManager FileManager { get; set; }
        protected IDirectoryManager DirectoryManager { get; set; }

        protected BaseManagerUsageTest()
        {
            Reconfigure();
        }

        protected void Reconfigure(bool useRootPath = false)
        {
            Adapter = new Mock<IAdapter>();
            AdapterManager = new AdapterManager();
            DirectoryManager = new DirectoryManager(AdapterManager);
            FileManager = new FileManager(AdapterManager);
            AdapterManager.Register(new AdapterRegistration
            {
                Adapter = Adapter.Object,
                RootPath = useRootPath ? "root/".AsBaselineFilesystemPath() : null
            });
        }
    }
}
