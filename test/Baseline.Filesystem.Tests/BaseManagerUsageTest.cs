using Baseline.Filesystem.Internal.Contracts;
using Moq;

namespace Baseline.Filesystem.Tests
{
    public abstract class BaseManagerUsageTest
    {
        protected Mock<IAdapter> Adapter { get;  }
        protected IAdapterManager AdapterManager { get;  }
        protected IFileManager FileManager { get;  }
        protected IDirectoryManager DirectoryManager { get;  }

        protected BaseManagerUsageTest()
        {
            Adapter = new Mock<IAdapter>();
            AdapterManager = new AdapterManager();
            DirectoryManager = new DirectoryManager(AdapterManager);
            FileManager = new FileManager(AdapterManager);
            AdapterManager.Register(Adapter.Object);
        }
    }
}
