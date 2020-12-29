namespace Baseline.Filesystem
{
    /// <summary>
    /// Thrown when a path provided by a consuming application contains an invalid character. As per the documentation,
    /// some characters are not allowed within paths as it cannot be guaranteed that every storage adapter supports
    /// them.
    /// </summary>
    public class PathContainsInvalidCharacterException : InvalidPathException
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="PathContainsInvalidCharacterException" />, with reference to the
        /// original path that caused the exception to be thrown.
        /// </summary>
        /// <param name="originalPath">The path that caused the exception to be thrown.</param>
        public PathContainsInvalidCharacterException(string originalPath)
            : base(originalPath,
                $"The path '{originalPath}' contains an invalid character which is not allowed in Baseline.Filesystem. For more " +
                "information, see the documentation."
            )
        {
        }
    }
}
