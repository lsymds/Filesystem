using System;

namespace LSymds.Filesystem;

/// <summary>
/// Base exception that is inherited by all LSymds.Filesystem thrown exceptions.
/// </summary>
public abstract class FilesystemException : Exception
{
    /// <summary>
    /// Initialises a new instance of the <see cref="FilesystemException"/> class.
    /// </summary>
    /// <param name="message">The message to include in the exception.</param>
    /// <param name="innerException">The inner exception to include in the exception.</param>
    protected FilesystemException(string message, Exception innerException = null)
        : base(message, innerException) { }
}
