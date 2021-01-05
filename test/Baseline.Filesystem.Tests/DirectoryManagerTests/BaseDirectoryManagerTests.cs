using Baseline.Filesystem.Internal.Contracts;
using Moq;

namespace Baseline.Filesystem.Tests.DirectoryManagerTests
{
    public abstract class BaseDirectoryManagerTests
    {
        protected Mock<IAdapter> Adapter { get;  }
        protected IAdapterManager AdapterManager { get;  }
        protected IDirectoryManager DirectoryManager { get;  }

        protected BaseDirectoryManagerTests()
        {
            Adapter = new Mock<IAdapter>();
            AdapterManager = new AdapterManager();
            DirectoryManager = new DirectoryManager(AdapterManager);
            AdapterManager.Register(Adapter.Object);
        }
    }
}
