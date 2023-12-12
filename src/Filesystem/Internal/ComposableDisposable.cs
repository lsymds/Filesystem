using System;

namespace LSymds.Filesystem.Internal;

/// <summary>
/// A composable implementation of the <see cref="IDisposable"/> interface.
/// </summary>
public class ComposableDisposable : IDisposable
{
    private readonly Action _dispose;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComposableDisposable"/> class.
    /// </summary>
    public ComposableDisposable(Action dispose)
    {
        _dispose = dispose;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _dispose();
    }
}
