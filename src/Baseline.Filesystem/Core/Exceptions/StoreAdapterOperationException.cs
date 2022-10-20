using System;

namespace Baseline.Filesystem;

/// <summary>
/// Wrapped, generic exception that is thrown when a store's adapter throws an exception during
/// an operation (i.e. writing a file) that isn't otherwise handled by Baseline.Filesystem (i.e. files not
/// being found throwing a FileNotFoundException). Prevents you from having to reference the provider's
/// SDK just to catch any exceptions.
/// </summary>
public class StoreAdapterOperationException : BaselineFilesystemException
{
    /// <summary>
    /// Initialises a new instance of the <see cref="StoreAdapterOperationException" />.
    /// </summary>
    /// <param name="message">The reason why the exception was thrown.</param>
    /// <param name="innerException">The exception that caused this wrapped one to be thrown (if any).</param>
    public StoreAdapterOperationException(string message, Exception innerException = null)
        : base(message, innerException) { }
}
