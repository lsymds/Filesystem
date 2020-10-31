using System;

namespace Storio
{
    /// <summary>
    /// Wrapped, generic exception that is thrown when an adapter (for example S3) throws an exception during an
    /// operation (i.e. writing a file).
    /// </summary>
    public class AdapterOperationException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AdapterOperationException" />.
        /// </summary>
        /// <param name="message">The reason why the exception was thrown.</param>
        /// <param name="innerException">The exception that caused this wrapped one to be thrown (if any).</param>
        protected AdapterOperationException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
