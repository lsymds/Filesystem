using System;
using System.Threading.Tasks;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Request used to iterate over the contents of a directory in a performant manner.
    /// </summary>
    public class IterateDirectoryContentsRequest : BaseSingleDirectoryRequest<IterateDirectoryContentsRequest>
    {
        /// <summary>
        /// Gets or sets a delegate to perform on each file or directory within the requested directory. Returning a
        /// `false` boolean from the delegate will stop iterating further.
        /// </summary>
        public Func<PathRepresentation, Task<bool>> Action { get; set; }

        /// <inheritdoc />
        internal override IterateDirectoryContentsRequest ShallowClone()
        {
            var cloned = base.ShallowClone();
            cloned.Action = Action;
            return cloned;
        }
    }
}
