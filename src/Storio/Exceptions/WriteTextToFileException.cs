using System;

namespace Storio
{
    /// <summary>
    /// Exception that is thrown when the WriteTextToFileAsync method throws an adapter specific exception.
    /// </summary>
    public class WriteTextToFileException : AdapterOperationException
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="WriteTextToFileException" />.
        /// </summary>
        /// <param name="message">The reason why the exception was thrown.</param>
        /// <param name="innerException">The exception that caused this wrapped one to be thrown (if any).</param>
        public WriteTextToFileException(string reason, Exception innerException = null)
            : base(reason, innerException)
        {
        }
    }
}
