using System.Collections.Generic;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Response for the ListDirectoryContentsAsync method. Contains a flat list of all available directories and files
    /// for the requested directory.
    /// </summary>
    public class ListDirectoryContentsResponse
    {
        /// <summary>
        /// Gets or sets the contents of the directory defined in the request.
        /// </summary>
        public List<PathRepresentation> Contents { get; set; }
    }
}
