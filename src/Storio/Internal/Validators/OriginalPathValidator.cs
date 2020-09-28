using System.Text.RegularExpressions;

namespace Storio.Internal.Validators
{
    /// <summary>
    /// Validation methods for original paths coming into the library.
    /// </summary>
    internal static class OriginalPathValidator
    {
        /// <summary>
        /// Validates the original path according to a set of criteria and potentially throws a number of
        /// validation exceptions if the checks do not pass.
        /// </summary>
        /// <param name="originalPath">The original path to validate.</param>
        public static void ValidateAndThrowIfUnsuccessful(string originalPath)
        {
            ThrowForBlankPath(originalPath);
            ThrowForInvalidCharacters(originalPath);
            ThrowForRelativePath(originalPath);
        }

        /// <summary>
        /// Checks the path and throws an exception if it is empty or whitespace.
        /// </summary>
        /// <param name="path">The path to check.</param>
        private static void ThrowForBlankPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new PathIsBlankException();
        }

        /// <summary>
        /// Checks the path for Storio-invalid characters and throws a validation exception if any are found.
        /// </summary>
        /// <param name="path">The path to check for invalid characters.</param>
        /// <exception cref="PathContainsInvalidCharacterException" />
        private static void ThrowForInvalidCharacters(string path)
        {
            const string invalidCharactersRegexString = @"[\*\""\\\[\]\:\;\|\,\<\>\'\$\£\%\^\(\)\+\=\!]";
            
            var invalidCharactersRegex = new Regex(invalidCharactersRegexString);
            if (invalidCharactersRegex.IsMatch(path))
                throw new PathContainsInvalidCharacterException(path);
        }

        /// <summary>
        /// Checks the path to see if it is relative. As per the documentation, paths in Storio are required to be
        /// absolute to ensure maximum transferability between the different adapters.
        /// </summary>
        /// <param name="path">The path to check to see if it is relative.</param>
        /// <exception cref="PathIsRelativeException" />
        private static void ThrowForRelativePath(string path)
        {
            if (path.StartsWith("~") || path.StartsWith("./") || path.StartsWith(".."))
                throw new PathIsRelativeException(path);
        }
    }
}
