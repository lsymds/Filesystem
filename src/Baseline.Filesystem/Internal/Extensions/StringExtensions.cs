using System;

namespace Baseline.Filesystem.Internal.Extensions;

/// <summary>
/// Contains extension methods related to strings.
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Replaces the first occurrence of a term within a string with a replacement. All other occurrences remain
    /// unchanged.
    /// </summary>
    /// <param name="source">The string to modify.</param>
    /// <param name="term">The original string to replace.</param>
    /// <param name="replacement">The string to replace the first occurrence of the original with.</param>
    /// <returns>
    /// The source string with the first occurrence of the original param replaced with the replacement param.
    /// </returns>
    public static string ReplaceFirstOccurrence(this string source, string term, string replacement)
    {
        var positionOfOriginal = source.IndexOf(term, StringComparison.Ordinal);
        if (positionOfOriginal < 0)
        {
            return source;
        }

        return source.Substring(0, positionOfOriginal)
            + replacement
            + source.Substring(positionOfOriginal + term.Length);
    }

    /// <summary>
    /// Replaces the last occurrence of a term within a string with a replacement. All other occurrences remain
    /// unchanged.
    /// </summary>
    /// <param name="source">The source string to modify.</param>
    /// <param name="term">The term to replace.</param>
    /// <param name="replacement">The string to replace the term with.</param>
    /// <returns>The modified string with the last occurrence of the term replaced with the replacement.</returns>
    public static string ReplaceLastOccurrence(this string source, string term, string replacement)
    {
        var positionOfOriginal = source.LastIndexOf(term, StringComparison.OrdinalIgnoreCase);
        if (positionOfOriginal < 0)
        {
            return source;
        }

        return source.Substring(0, positionOfOriginal)
            + replacement
            + source.Substring(positionOfOriginal + term.Length);
    }
}
