using System.Collections.Generic;
using System.Linq;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Builds a single <see cref="PathRepresentation" /> from one or more paths.
    /// </summary>
    public class PathCombinationBuilder
    {
        private readonly IEnumerable<PathRepresentation> _paths;

        /// <summary>
        /// Initialises a new instance of the <see cref="PathCombinationBuilder" /> with reference to the paths to
        /// combine.
        /// </summary>
        /// <param name="paths">The paths to combine.</param>
        public PathCombinationBuilder(params PathRepresentation[] paths)
        {
            _paths = paths;
        }

        /// <summary>
        /// Builds a unified <see cref="PathRepresentation" /> from one or more paths.
        /// </summary>
        /// <returns>The unified <see cref="PathRepresentation" />.</returns>
        public PathRepresentation Build()
        {
            var combinedPath = string.Join("/", _paths.Select(x => x.NormalisedPath));
            return new PathRepresentationBuilder(combinedPath).Build();
        }
    }
}
