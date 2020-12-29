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
            var (directories, finalPathPart) = DirectoriesAndFinalPathPartFromSplitPath(normalisedPath.Split('/'));

            return new PathRepresentation
            {
                DirectoryLevels = (uint) directories.Count,
                DirectoryPath = DirectoryPathFromDirectories(directories),
                DirectoryTree = DirectoryTreeFromDirectories(directories),
                Extension = ExtensionFromFinalPathPart(finalPathPart),
                FinalPathPart = finalPathPart,
                FinalPathPartIsObviouslyADirectory = path.EndsWith("/"),
                NormalisedPath = normalisedPath,
                OriginalPath = path
            };
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

        /// <summary>
        /// Retrieves the directory tree (i.e. users, users/Foo, users/Foo/Bar) from a collection of directory parts.
        /// This is useful for adapters that have to create each directory and subdirectory serially.
        /// </summary>
        /// <param name="directories">The collection of directory parts.</param>
        /// <returns>The sorted directory tree.</returns>
        private static SortedSet<string> DirectoryTreeFromDirectories(IReadOnlyCollection<string> directories)
        {
            var set = new SortedSet<string>();
            var workingDirectory = string.Empty;

            for (var i = 0; i < directories.Count; i++)
            {
                workingDirectory += $"{(i == 0 ? string.Empty : "/")}{directories.ElementAt(i)}";
                set.Add(workingDirectory);
            }

            return set;
        }

        /// <summary>
        /// Retrieves the full directory path (including a trailing separator) from a collection of directory parts.
        /// </summary>
        /// <param name="directories">The collection of directory parts.</param>
        /// <returns>The full directory path including a trailing separator.</returns>
        private static string DirectoryPathFromDirectories(IReadOnlyCollection<string> directories)
        {
            return directories.Any() ? $"{string.Join("/", directories)}" : null;
        }

        /// <summary>
        /// Extracts the assumed file extension from a final path part.
        ///
        /// Where a file begins with a . and contains no further .s, it's likely just a hidden file or folder with
        /// no extension, for example .npmrc.
        ///
        /// Where a file begins with a . and contains further s', it's likely it's an actual file, for example
        /// .nuget.config.
        /// </summary>
        /// <param name="finalPathPart">The final path part from the original, now normalised path.</param>
        /// <returns>The assumed file extension.</returns>
        private static string ExtensionFromFinalPathPart(string finalPathPart)
        {
            var numberOfExtensionSeparators = finalPathPart.Count(x => x == '.');
            if (numberOfExtensionSeparators == 0 || numberOfExtensionSeparators == 1 && finalPathPart.StartsWith("."))
                return null;

            return finalPathPart.Split('.').Last();
        }
    }
}
