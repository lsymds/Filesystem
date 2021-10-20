using System;
using System.Collections.Generic;
using System.Linq;
using Baseline.Filesystem.Internal.Extensions;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Representation of an adapter agnostic path.
    /// </summary>
    public class PathRepresentation
    {
        /// <summary>
        /// Gets a function that returns the deeply nested path tree of the normalised path of the current path
        /// representation. This is stored as a function to stop the unneccessary and complex lookups when creating
        /// what is otherwise a light object.
        /// <example>
        /// For the path a/b/c/d/e/f/g/.keep, <see cref="GetPathTree"/> would return the following:
        /// <list type="bullet">
        ///     <item>
        ///         <description>a/</description>
        ///     </item>
        ///     <item>
        ///         <description>a/b/</description>
        ///     </item>
        ///     <item>
        ///         <description>a/b/c/</description>
        ///     </item>
        ///     <item>
        ///         <description>a/b/c/d/</description>
        ///     </item>
        ///     <item>
        ///         <description>a/b/c/d/e/</description>
        ///     </item>
        ///     <item>
        ///         <description>a/b/c/d/e/f/</description>
        ///     </item>
        ///     <item>
        ///         <description>a/b/c/d/e/f/g/</description>
        ///     </item>
        ///     <item>
        ///         <description>a/b/c/d/e/f/g/.keep</description>
        ///     </item>
        /// </list>
        /// </example>
        /// </summary>
        public Func<IEnumerable<PathRepresentation>> GetPathTree { get; internal set; }
        
        /// <summary>
        /// Gets the final part of the path. This could be a directory or it could be a file. It is up to the individual
        /// managers ({directory, file}) to decide how this final part of the path is used.
        /// </summary>
        public string FinalPathPart { get; internal set; }

        /// <summary>
        /// Gets the normalised version of the path used throughout Baseline.Filesystem.
        /// </summary>
        public string NormalisedPath { get; internal set; }
        
        /// <summary>
        /// Gets the original path that was specified by the consuming application.
        /// </summary>
        public string OriginalPath { get; internal set; }

        /// <summary>
        /// Gets whether or not the final path part was obviously intended to be a directory (i.e. it ended with a
        /// terminating slash).
        /// </summary>
        public bool FinalPathPartIsObviouslyADirectory => FinalPathPart.EndsWith("/");

        /// <summary>
        /// Retrieves the extension of the path (if it has one) prefixed with a '.' (i.e. .xml). Will return an empty
        /// string if there is not obviously an extension.
        /// </summary>
        public string Extension
        {
            get
            {
                if (
                    FinalPathPartIsObviouslyADirectory || 
                    string.IsNullOrWhiteSpace(FinalPathPart) || 
                    !FinalPathPart.Contains(".")
                )
                {
                    return string.Empty;
                }

                var splitPaths = FinalPathPart.Split(".", StringSplitOptions.RemoveEmptyEntries);

                if (splitPaths.Length < 2)
                {
                    return null;
                }

                return $".{splitPaths.Last().ToLower()}";
            }
        }

        /// <summary>
        /// Gets the file name without the extension. Where the path is obviously a directory, an empty string will
        /// be returned.
        /// </summary>
        public string FileNameWithoutExtension
        {
            get
            {
                if (FinalPathPartIsObviouslyADirectory)
                {
                    return string.Empty;
                }

                return FinalPathPart.ReplaceLastOccurrence(Extension, string.Empty);
            }
        }

        /// <summary>
        /// Compares two paths and identifies if they're equal.
        /// </summary>
        /// <param name="left">The left hand side of the == comparison.</param>
        /// <param name="right">The right hand side of the == comparison.</param>
        public static bool operator ==(PathRepresentation left, PathRepresentation right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                return false;
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Compares two paths and identifies if they're not equal.
        /// </summary>
        /// <param name="left">The left hand side of the != comparison.</param>
        /// <param name="right">The right hand side of the != comparison.</param>
        public static bool operator !=(PathRepresentation left, PathRepresentation right) => !(left == right);
        
        /// <summary>
        /// Identifies and returns whether the current <see cref="PathRepresentation"/> equals the one specified.
        /// </summary>
        /// <param name="other">The path representation to compare to the current one.</param>
        protected bool Equals(PathRepresentation other)
        {
            return NormalisedPath == other.NormalisedPath;
        }

        /// <summary>
        /// Identifies and returns whether an unidentified object is equal to the current <see cref="PathRepresentation"/>
        /// instance.
        /// </summary>
        /// <param name="obj">The object to compare to the current <see cref="PathRepresentation"/> instance.</param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PathRepresentation) obj);
        }

        /// <summary>
        /// Retrieves the hash code for the current <see cref="PathRepresentation" /> instance.
        /// </summary>
        public override int GetHashCode()
        {
            return NormalisedPath.GetHashCode();
        }

        /// <summary>
        /// Combines the current path representation with a base path representation, wherein the base path
        /// representation is set first, and the current path after.
        /// </summary>
        /// <param name="base">The base path to combine with the current path.</param>
        /// <returns>The newly combined path.</returns>
        internal PathRepresentation CombineWithBase(PathRepresentation @base)
        {
            return new PathCombinationBuilder(@base, this).Build();
        }
    }
}
