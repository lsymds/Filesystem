using System;

namespace Baseline.Filesystem.Internal.Extensions
{
    /// <summary>
    /// Contains extension methods related to strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces the first occurrence of a term within a string with a replacement. All other occurrences remain
        /// unchanged.
        /// </summary>
        /// <param name="source">The string to modify.</param>
        /// <param name="original">The original string to replace.</param>
        /// <param name="replacement">The string to replace the first occurrence of the original with.</param>
        /// <returns>
        /// The source string with the first occurrence of the original param replaced with the replacement param.
        /// </returns>
        public static string ReplaceFirstOccurrence(this string source, string original, string replacement)
        {
            var positionOfOriginal = source.IndexOf(original, StringComparison.Ordinal);
            if (positionOfOriginal < 0)
            {
                return source;
            }
            
            return source.Substring(0, positionOfOriginal) + 
                   replacement + 
                   source.Substring(positionOfOriginal + original.Length);
        }
    }
}
