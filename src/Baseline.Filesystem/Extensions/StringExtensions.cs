namespace Baseline.Filesystem;

/// <summary>
/// Extension methods related to the <see cref="string"/> type.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts a string representation of a path into one usable by Baseline.Filesystem.
    /// </summary>
    /// <param name="path">The string representation of the path.</param>
    /// <returns>
    /// The Baseline.Filesystem usable <see cref="PathRepresentation" /> containing details extracted from the original path.
    /// </returns>
    public static PathRepresentation AsBaselineFilesystemPath(this string path)
    {
        return new PathRepresentationBuilder(path).Build();
    }
}
