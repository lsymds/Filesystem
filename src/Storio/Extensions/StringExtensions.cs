namespace Storio
{
    /// <summary>
    /// Extension methods related to the <see cref="string"/> type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string representation of a path into one usable by Storio.
        /// </summary>
        /// <param name="path">The string representation of the path.</param>
        /// <returns>
        /// The Storio usable <see cref="PathRepresentation" /> containing details extracted from the original path.
        /// </returns>
        public static PathRepresentation AsStorioPath(this string path)
        {
            return new PathRepresentationBuilder(path).Build();
        }
    }
}
