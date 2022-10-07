using System.Collections.Generic;
using System.Linq;

namespace Baseline.Filesystem.Internal.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="PathRepresentation"/> class.
    /// </summary>
    internal static class PathRepresentationExtensions
    {
        /// <summary>
        /// Removes the root path from an single path representations.
        /// </summary>
        /// <param name="source">A path representation to remove the root path from.</param>
        /// <param name="rootPath">The root path to remove.</param>
        /// <returns>A new path with the root path removed.</returns>
        public static PathRepresentation RemoveRootPath(
            this PathRepresentation source,
            PathRepresentation rootPath
        )
        {
            return new[] { source }.RemoveRootPath(rootPath).First();
        }

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
                // Remove any occurrences where the path is equivalent to a representation of a root path. For example,
                // if there was a root path of a/b and pathRepresentation was a/b, it would be filtered out and not
                // returned.
                if (rootPath.GetPathTree().Any(x => x == pathRepresentation))
                {
                    continue;
                }

                var replacementPathRepresentation =
                    pathRepresentation.FinalPathPartIsObviouslyADirectory
                        ? (pathRepresentation.NormalisedPath + "/").ReplaceFirstOccurrence(
                            rootPath.NormalisedPath + "/",
                            string.Empty
                        )
                        : pathRepresentation.NormalisedPath.ReplaceFirstOccurrence(
                            rootPath.NormalisedPath + "/",
                            string.Empty
                        );

                if (string.IsNullOrWhiteSpace(replacementPathRepresentation))
                {
                    continue;
                }

                yield return replacementPathRepresentation.AsBaselineFilesystemPath();
            }
        }
    }
}
