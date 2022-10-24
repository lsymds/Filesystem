namespace Baseline.Filesystem.Internal.Adapters.S3;

/// <summary>
/// Extension methods for the <see cref="PathRepresentation"/> class.
/// </summary>
internal static class PathRepresentationExtensions
{
    /// <summary>
    /// Gets a path that is safe to be used to represent a directory within S3. They don't really exist, but whatevs.
    /// Throws an exception if the path is not obviously a directory as an additional safeguard.
    /// </summary>
    /// <param name="pathRepresentation">The path representation to convert into an S3 safe directory path.</param>
    public static string S3SafeDirectoryPath(this PathRepresentation pathRepresentation)
    {
        if (!pathRepresentation.FinalPathPartIsADirectory)
        {
            throw new PathIsNotObviouslyADirectoryException(pathRepresentation.OriginalPath);
        }

        return $"{pathRepresentation.NormalisedPath}/";
    }
}
