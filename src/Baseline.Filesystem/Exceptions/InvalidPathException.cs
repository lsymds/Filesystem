using System;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Base exception class used to provide a base exception class consuming applications can use.
    /// </summary>
    public abstract class InvalidPathException : Exception
    {
        /// <summary>
        /// The path specified that is invalid.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="InvalidPathException" /> class with reference to the original
        /// path and a message to use.
        /// </summary>
        /// <param name="path">The original path that is invalid.</param>
        /// <param name="message">The message to set against the exception.</param>
        protected InvalidPathException(string path, string message)
            : base(message)
        {
            Path = path;
        }
    }
}
