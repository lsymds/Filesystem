using System;
using System.Threading;
using System.Threading.Tasks;
using Baseline.Filesystem.Internal;

namespace Baseline.Filesystem;

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
        _configuration = configuration;
    }

    /// <summary>
    /// Locks the filesystem but returns an IDisposable that can be disposed of to release the lock. Use this with a
    /// using statement for cleaner code.
    /// </summary>
    private async Task<IDisposable> LockFilesystemAsync()
    {
        await _mutex.WaitAsync();
        return new ComposableDisposable(() => _mutex.Release());
    }
}
