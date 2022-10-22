using System.Threading.Tasks;
using FluentAssertions;

namespace Baseline.Filesystem.Tests.Adapters.S3;

public abstract class BaseIntegrationTest
{
    protected IIntegrationTestAdapter TestAdapter;
    protected IFileManager FileManager;
    protected IDirectoryManager DirectoryManager;

    public async ValueTask DisposeAsync()
    {
        if (TestAdapter != null)
        {
            await TestAdapter.DisposeAsync();
        }
    }

    protected async Task ExpectFileToExistAsync(PathRepresentation path)
    {
        var exists = await TestAdapter.FileExistsAsync(path);
        exists.Should().BeTrue();
    }

    protected async Task ExpectFileNotToExistAsync(PathRepresentation path)
    {
        var exists = await TestAdapter.FileExistsAsync(path);
        exists.Should().BeFalse();
    }

    protected async Task ExpectDirectoryToExistAsync(PathRepresentation path)
    {
        var exists = await TestAdapter.DirectoryExistsAsync(path);
        exists.Should().BeTrue();
    }

    protected async Task ExpectDirectoryNotToExistAsync(PathRepresentation path)
    {
        var exists = await TestAdapter.DirectoryExistsAsync(path);
        exists.Should().BeFalse();
    }

    protected async Task ConfigureTestAsync(Adapter toUse, bool useRootPath = false)
    {
        await DisposeAsync();

        var rootPath = useRootPath
            ? $"{TestUtilities.RandomString(6)}/{TestUtilities.RandomString(2)}/"
            : null;

        TestAdapter = toUse switch
        {
            Adapter.S3 => new S3IntegrationTestAdapter(rootPath)
        };

        var adapterManager = new StoreManager();
        adapterManager.Register(
            new StoreRegistration
            {
                Adapter = await TestAdapter.BootstrapAsync(),
                RootPath = rootPath?.AsBaselineFilesystemPath()
            }
        );

        FileManager = new FileManager(adapterManager);
        DirectoryManager = new DirectoryManager(adapterManager);
    }
}
