namespace Baseline.Filesystem
{
    /// <summary>
    /// Representation of an adapter agnostic path.
    /// </summary>
    public class PathRepresentation
    {
        /// <summary>
        /// Gets the final part of the path. This could be a directory or it could be a file. It is up to the individual
        /// managers ({directory, file}) to decide how this final part of the path is used.
        /// </summary>
        public string FinalPathPart { get; internal set; }
        
        /// <summary>
        /// Gets whether or not the final path part was obviously intended to be a directory (i.e. it ended with a
        /// terminating slash).
        /// </summary>
        public bool FinalPathPartIsObviouslyADirectory { get; internal set; }
        
        /// <summary>
        /// Gets the normalised version of the path used throughout Baseline.Filesystem.
        /// </summary>
        public string NormalisedPath { get; internal set; }
        
        /// <summary>
        /// Gets the original path that was specified by the consuming application.
        /// </summary>
        public string OriginalPath { get; internal set; }

        /// <summary>
        /// Combines the current path representation with a base path representation, wherein the base path
        /// representation is set first, and the current path after.
        /// </summary>
        /// <param name="@base">The base path to combine with the current path.</param>
        /// <returns>The newly combined path.</returns>
        internal PathRepresentation CombineWithBase(PathRepresentation @base)
        {
            return new PathCombinationBuilder(@base, this).Build();
        }
    }
}
