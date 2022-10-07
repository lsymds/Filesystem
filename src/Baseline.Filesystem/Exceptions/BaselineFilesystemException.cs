using System;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Base exception that is inherited by all Baseline thrown exceptions.
    /// </summary>
    public abstract class BaselineFilesystemException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="BaselineFilesystemException"/> class.
        /// </summary>
        /// <param name="message">The message to include in the exception.</param>
        /// <param name="innerException">The inner exception to include in the exception.</param>
        protected BaselineFilesystemException(string message, Exception innerException = null)
            : base(message, innerException) { }
    }
}
