namespace LSymds.Filesystem.Tests.Core.StoreManagerTests;

public abstract class BaseStoreManagerTest
{
    protected IStoreManager StoreManager { get; }

    protected BaseStoreManagerTest()
    {
        StoreManager = new StoreManager();
    }
}
