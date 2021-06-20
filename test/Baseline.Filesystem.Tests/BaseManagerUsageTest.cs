using Moq;

#nullable enable

namespace Baseline.Filesystem.Tests
{
    public abstract class BaseManagerUsageTest
    {
        protected Mock<IAdapter> Adapter { get; set; } = null!;
        protected IAdapterManager AdapterManager { get; set; } = null!;
        protected IFileManager FileManager { get; set; } = null!;
        protected IDirectoryManager DirectoryManager { get; set; } = null!;

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
