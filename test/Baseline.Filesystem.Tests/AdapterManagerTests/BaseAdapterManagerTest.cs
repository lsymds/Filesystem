namespace Baseline.Filesystem.Tests.AdapterManagerTests
{
    public abstract class BaseAdapterManagerTest
    {
        protected IAdapterManager AdapterManager { get; }

        protected BaseAdapterManagerTest()
        {
            AdapterManager = new AdapterManager();
        }
    }
}
