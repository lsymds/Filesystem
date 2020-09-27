using Storio.Tests.Fixtures;

namespace Storio.Tests.FileManagerTests
{
    public abstract class BaseFileManagerTests
    {
        protected IAdapterManager AdapterManager { get;  }
        protected IFileManager FileManager { get;  }

        protected BaseFileManagerTests()
        {
            AdapterManager = new AdapterManager();
            FileManager = new FileManager(AdapterManager);
            
            AdapterManager.Register(new SuccessfulOutcomeAdapter());
        }
    }
}
