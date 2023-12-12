using System;

namespace LSymds.Filesystem.Internal.Validators.Directories;

/// <summary>
/// Validation methods for paths in directory requests.
/// </summary>
internal static class DirectoryPathValidator
{
    /// <summary>
    /// Validates a path that is supposedly a directory path and throws exceptions if it is not valid.
    /// </summary>
    /// <param name="directoryPath">The directory path to validate.</param>
    public static void ValidateAndThrowIfUnsuccessful(PathRepresentation directoryPath)
    {
        if (directoryPath == null)
        {
            throw new ArgumentNullException(nameof(directoryPath));
        }

        if (!directoryPath.FinalPathPartIsADirectory)
        {
            throw new PathIsNotObviouslyADirectoryException(directoryPath.OriginalPath);
        }
    }
}
