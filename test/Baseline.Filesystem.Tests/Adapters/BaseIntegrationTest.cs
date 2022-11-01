using System;
using System.Threading.Tasks;
using FluentAssertions;

namespace Baseline.Filesystem.Tests.Adapters;

public abstract class BaseIntegrationTest : IAsyncDisposable
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

    protected async Task ExpectPublicUrlContainsTextFromAdapter(string url, PathRepresentation path)
    {
        var requiredTexts = await TestAdapter.TextThatShouldBeInPublicUrlForPathAsync(path);
        url.Should().ContainAll(requiredTexts);
    }

    /// <summary>
    /// Configures the test for a given adapter and root path.
    ///
    /// Whilst adapters are technically root path agnostic (as in, they do not know that a root path exists), the paths
    /// they interact with could be based off of a root path. Thus, when creating or validating test data, we
    /// need to know about that root path so we can interact with the same underlying path.
    /// </summary>
    protected async Task ConfigureTestAsync(Adapter toUse, bool useRootPath = false)
    {
        // It's possible this method could be called multiple times, so dispose the old resources before we create any
        // new ones.
        await DisposeAsync();

        var rootPath = useRootPath
            ? $"{TestUtilities.RandomString(6)}/{TestUtilities.RandomString(2)}/".AsBaselineFilesystemPath()
            : null;

        TestAdapter = toUse switch
        {
            Adapter.S3 => new S3IntegrationTestAdapter(rootPath),
            Adapter.Memory => new MemoryIntegrationTestAdapter(rootPath),
            Adapter.Local => new LocalIntegrationTestAdapter(rootPath),
            _ => throw new ArgumentOutOfRangeException(nameof(toUse), toUse, null)
        };

        var adapterManager = new StoreManager();
        adapterManager.Register(
            new StoreRegistration
            {
                Adapter = await TestAdapter.BootstrapAsync(),
                RootPath = rootPath
            }
        );

        FileManager = new FileManager(adapterManager);
        DirectoryManager = new DirectoryManager(adapterManager);
    }
}
