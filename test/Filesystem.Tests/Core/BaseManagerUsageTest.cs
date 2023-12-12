#nullable enable

using Moq;

namespace LSymds.Filesystem.Tests.Core;

public abstract class BaseManagerUsageTest
{
    protected Mock<IAdapter> Adapter { get; set; } = null!;
    protected IStoreManager StoreManager { get; set; } = null!;
    protected IFileManager FileManager { get; set; } = null!;
    protected IDirectoryManager DirectoryManager { get; set; } = null!;

    protected BaseManagerUsageTest()
    {
        Reconfigure();
    }

    protected void Reconfigure(bool useRootPath = false)
    {
        Adapter = new Mock<IAdapter>();
        StoreManager = new StoreManager();
        DirectoryManager = new DirectoryManager(StoreManager);
        FileManager = new FileManager(StoreManager);
        StoreManager.Register(
            new StoreRegistration
            {
                Adapter = Adapter.Object,
                RootPath = useRootPath ? "root/".AsBaselineFilesystemPath() : null
            }
        );
    }
}
