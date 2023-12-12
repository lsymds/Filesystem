using System.Collections.Generic;
using System.Linq;
using LSymds.Filesystem.Internal.Validators;

namespace LSymds.Filesystem;

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

        return new PathRepresentation
        {
            GetPathTree = () => BuildPathTree(path),
            FinalPathPart = normalisedPath.Split('/').Last(),
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

        var pathSplitByDirectory = path.Split("/")
            .Where(pathSplit => !string.IsNullOrEmpty(pathSplit))
            .ToList();
        var currentPath = string.Empty;

        for (var i = 0; i < pathSplitByDirectory.Count; i++)
        {
            // If current iteration is the last path part and the last path part didn't originally end in a /, add as a
            // file.
            if (i == pathSplitByDirectory.Count - 1 && !path.EndsWith("/"))
            {
                currentPath += pathSplitByDirectory[i];
            }
            // Otherwise, add as a directory.
            else
            {
                currentPath += pathSplitByDirectory[i] + '/';
            }

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
}
