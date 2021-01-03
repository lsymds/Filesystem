using Baseline.Filesystem.Internal.Contracts;
using Moq;

namespace Baseline.Filesystem.Tests.FileManagerTests
{
    public abstract class BaseFileManagerTests
    {
        protected Mock<IAdapter> Adapter { get;  }
        protected IAdapterManager AdapterManager { get;  }
        protected IFileManager FileManager { get;  }

        protected BaseFileManagerTests()
        {
            Adapter = new Mock<IAdapter>();
            AdapterManager = new AdapterManager();
            FileManager = new FileManager(AdapterManager);
            AdapterManager.Register(Adapter.Object);
        }
    }
}
