namespace Baseline.Filesystem
{
    /// <summary>
    /// Thrown when a null, empty or whitespace path is provided to the path builder.
    /// </summary>
    public class PathIsBlankException : InvalidPathException
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="PathIsBlankException"/> class.
        /// </summary>
        public PathIsBlankException()
            : base(
                string.Empty,
                "The path provided is null, empty or whitespace. Not a lot we can do with that!"
            ) { }
    }
}
