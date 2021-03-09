using System.Collections.Generic;

namespace Baseline.Filesystem.Internal.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="PathRepresentation"/> class.
    /// </summary>
    internal static class PathRepresentationExtensions
    {
        /// <summary>
        /// Removes the root path from an enumerable collection of path representations.
        /// </summary>
        /// <param name="source">A collection of path representations to remove the root path from.</param>
        /// <param name="rootPath">The root path to remove.</param>
        /// <returns>A new collection with the root path removed from each path representation.</returns>
        public static IEnumerable<PathRepresentation> RemoveRootPath(
            this IEnumerable<PathRepresentation> source,
            PathRepresentation rootPath
        )
        {
            foreach (var pathRepresentation in source)
            {
                var replacementPathRepresentation = pathRepresentation.NormalisedPath
                    .ReplaceFirstOccurrence(rootPath.NormalisedPath + "/", string.Empty);

                if (string.IsNullOrWhiteSpace(replacementPathRepresentation))
                {
                    continue;
                }

                yield return replacementPathRepresentation.AsBaselineFilesystemPath();
            }
        }
    }
}
