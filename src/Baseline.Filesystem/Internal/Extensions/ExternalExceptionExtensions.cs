using System;
using System.Threading.Tasks;

namespace Baseline.Filesystem.Internal.Extensions
{
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
        /// <param name="adapterName">The name of the adapter that the exception occurred in.</param>
        public static async Task WrapExternalExceptionsAsync(this Task task, string adapterName)
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

                throw e.CreateAdapterException(adapterName);
            }
        }
        
        /// <summary>
        /// Wraps any external exceptions thrown from the task and, if it's a Baseline exception rethrows it, but if
        /// it's an unmanaged exception wraps it.
        /// </summary>
        /// <param name="task">The asynchronous task that could potentially throw an external exception.</param>
        /// <param name="adapterName">The name of the adapter that the exception occurred in.</param>
        /// <returns>
        /// A task yielding the same response as the <see cref="task" /> parameter providing it does not throw
        /// any exceptions.
        /// </returns>
        public static async Task<TResponse> WrapExternalExceptionsAsync<TResponse>(
            this Task<TResponse> task,
            string adapterName
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
                
                throw e.CreateAdapterException(adapterName);
            }
        }
        
        /// <summary>
        /// Creates a <see cref="AdapterProviderOperationException"/> for a given exception and adapter name with a
        /// consistent message.
        /// </summary>
        /// <param name="e">The exception to use as the inner exception.</param>
        /// <param name="adapterName">The name of the adapter the exception was thrown from.</param>
        /// <returns>An <see cref="AdapterProviderOperationException"/> which wraps the original exception.</returns>
        private static AdapterProviderOperationException CreateAdapterException(this Exception e, string adapterName)
        {
            return new AdapterProviderOperationException(
                $"Unhandled exception thrown from adapter ({adapterName}), potentially whilst communicating with its API. See the inner exception for details.",
                e
            );
        }
    }
}
