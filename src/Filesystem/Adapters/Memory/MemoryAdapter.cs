using System;
using System.Threading;
using System.Threading.Tasks;
using LSymds.Filesystem.Internal;

namespace LSymds.Filesystem;

/// <summary>
/// Provides the shared, directory/file agnostic functions of the <see cref="IAdapter"/> implementation within memory.
/// Perfect for tests or systems that need short-lived, ephemeral data stores.
/// </summary>
public partial class MemoryAdapter : IAdapter
{
    private readonly MemoryAdapterConfiguration _configuration;
    private readonly SemaphoreSlim _mutex = new(1);

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryAdapter"/> class.
    /// </summary>
    public MemoryAdapter(MemoryAdapterConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration.PublicUrlToReturn))
        {
            throw new ArgumentNullException(
                nameof(configuration.PublicUrlToReturn),
                "A public URL to return is required. It can be anything, we just don't want to make any assumptions for you!"
            );
        }

        _configuration = configuration;
    }

    /// <summary>
    /// Locks the filesystem but returns an IDisposable that can be disposed of to release the lock. Use this with a
    /// using statement for cleaner code.
    /// </summary>
    private async Task<IDisposable> LockFilesystemAsync()
    {
        await _mutex.WaitAsync().ConfigureAwait(false);
        return new ComposableDisposable(() => _mutex.Release());
    }
}
