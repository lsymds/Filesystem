using System;
using System.Collections.Generic;
using System.Linq;
using Baseline.Filesystem.Internal.Validators;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Builds a <see cref="PathRepresentation" /> from a string representation of a path.
    /// </summary>
    public class PathRepresentationBuilder
    {
        private readonly string _originalPath;
        
        /// <summary>
        /// Initialises a new instance of the <see cref="PathRepresentationBuilder" /> class, referencing the original
        /// path.
        /// </summary>
        /// <param name="originalPath">The original path used to build the <see cref="PathRepresentation"/>.</param>
        public PathRepresentationBuilder(string originalPath)
        {
            _originalPath = originalPath;
        }

        /// <summary>
        /// Takes the original path represented as a string, validates it, then converts it into a
        /// <see cref="PathRepresentation"/>.
        /// </summary>
        /// <returns>The converted <see cref="PathRepresentation"/>.</returns>
        public PathRepresentation Build()
        {
            OriginalPathValidator.ValidateAndThrowIfUnsuccessful(_originalPath);
            return BuildPathRepresentation(_originalPath);
        }

        /// <summary>
        /// Builds a <see cref="PathRepresentation" /> from an original, string based path.
        /// </summary>
        /// <param name="path">The original path specified.</param>
        /// <returns>The <see cref="PathRepresentation" /> of the original path.</returns>
        private static PathRepresentation BuildPathRepresentation(string path)
        {
            var normalisedPath = NormalisePath(path);
            var (_, finalPathPart) = DirectoriesAndFinalPathPartFromSplitPath(normalisedPath.Split('/'));

            return new PathRepresentation
            {
                GetPathTree = () => BuildPathTree(path),
                FinalPathPart = finalPathPart,
                NormalisedPath = normalisedPath,
                OriginalPath = path
            };
        }

        /// <summary>
        /// Builds and returns the nested path tree for a specified path.
        /// </summary>
        /// <param name="path">The path to formulate into a path tree.</param>
        private static IEnumerable<PathRepresentation> BuildPathTree(string path)
        {
            if (!path.Contains("/"))
            {
                yield return path.AsBaselineFilesystemPath();
                yield break;
            }

            var pathSplitByDirectory = path.Substring(0, path.LastIndexOf("/", StringComparison.Ordinal) + 1).Split("/");
            var currentPath = string.Empty;

            foreach (var p in pathSplitByDirectory.Where(pathSplit => !string.IsNullOrEmpty(pathSplit)))
            {
                currentPath += p + "/";
                yield return currentPath.AsBaselineFilesystemPath();
            }
        }

        /// <summary>
        /// Normalises a path into one that is safe to use throughout the rest of the library.
        /// </summary>
        /// <param name="path">The path to normalise.</param>
        /// <returns>A normalised version of the original path specified.</returns>
        private static string NormalisePath(string path)
        {
            var normalisedPath = path;

            if (normalisedPath.StartsWith("/"))
                normalisedPath = normalisedPath.Substring(1);

            if (normalisedPath.EndsWith("/"))
                normalisedPath = normalisedPath.Substring(0, normalisedPath.Length - 1);

            return normalisedPath;
        }

        /// <summary>
        /// Retrieves the directories and the final path part from a collection of path parts. A final path part
        /// is the very final part of the path which could be a file or directory. This builder does not know or care
        /// what the final part truly is, just that it IS the final part.
        /// </summary>
        /// <param name="splitPath">The collection of path parts.</param>
        /// <returns>A tuple yielding the directories and the final path part.</returns>
        private static (List<string> directories, string finalPathPart) DirectoriesAndFinalPathPartFromSplitPath(
            IReadOnlyCollection<string> splitPath
        )
        {
            return (splitPath.Take(splitPath.Count - 1).ToList(), splitPath.Last());
        }
    }
}
