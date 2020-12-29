using System.Collections.Generic;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Representation of an adapter agnostic path.
    /// </summary>
    public class PathRepresentation
    {
        /// <summary>
        /// Gets the number of directory levels present within a path.
        /// </summary>
        public uint DirectoryLevels { get; internal set; }
        
        /// <summary>
        /// Gets the directory part of the file path.
        /// </summary>
        public string DirectoryPath { get; internal set; }
        
        /// <summary>
        /// Gets the directory tree which can be used to create each directory in turn should that be required.
        /// For example, if the directory path is users/Foo/Bar, the directory tree would be a sorted set with 3
        /// elements:
        ///     - users
        ///     - users/Foo
        ///     - users/Foo/Bar
        /// </summary>
        public SortedSet<string> DirectoryTree { get; internal set; }
        
        /// <summary>
        /// Gets the extension of the file if an extension was specified.
        /// </summary>
        public string Extension { get; internal set; }
        
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
    }
}
