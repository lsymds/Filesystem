using System;
using System.Threading.Tasks;

namespace Baseline.Filesystem.Internal.Extensions;

/// <summary>
/// Extension methods related to the handling of external exceptions.
/// </summary>
internal static class ExternalExceptionExtensions
{
    /// <summary>
    /// Wraps any external exceptions thrown from the task and, if it's a Baseline exception rethrows it, but if
    /// it's an unmanaged exception wraps it.
    /// </summary>
    /// <param name="task">The asynchronous task that could potentially throw an external exception.</param>
    /// <param name="storeName">The name of the store that the exception occurred in.</param>
    public static async Task WrapExternalExceptionsAsync(this Task task, string storeName)
    {
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception e)
        {
            if (e is BaselineFilesystemException)
            {
                throw;
            }

            throw e.CreateStoreAdapterException(storeName);
        }
    }

    /// <summary>
    /// Wraps any external exceptions thrown from the task and, if it's a Baseline exception rethrows it, but if
    /// it's an unmanaged exception wraps it.
    /// </summary>
    /// <param name="task">The asynchronous task that could potentially throw an external exception.</param>
    /// <param name="storeName">The name of the store that the exception occurred in.</param>
    public static async Task<TResponse> WrapExternalExceptionsAsync<TResponse>(
        this Task<TResponse> task,
        string storeName
    )
    {
        try
        {
            return await task.ConfigureAwait(false);
        }
        catch (Exception e)
        {
            if (e is BaselineFilesystemException)
            {
                throw;
            }

            throw e.CreateStoreAdapterException(storeName);
        }
    }

    /// <summary>
    /// Creates a <see cref="StoreAdapterOperationException"/> for a given exception and store name with a
    /// consistent message.
    /// </summary>
    /// <param name="e">The exception to use as the inner exception.</param>
    /// <param name="storeName">The name of the store the exception was thrown from.</param>
    private static StoreAdapterOperationException CreateStoreAdapterException(
        this Exception e,
        string storeName
    )
    {
        return new StoreAdapterOperationException(
            $"Unhandled exception thrown from the adapter for store '{storeName}', potentially whilst communicating "
            + $"with its API. See the inner exception for details.",
            e
        );
    }
}
