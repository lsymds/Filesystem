using System;
using System.Threading.Tasks;

namespace Baseline.Filesystem
{
    /// <summary>
    /// A delegate signature used to exit the iteration of a directory's contents.
    /// </summary>
    public delegate void ExitIteration();
    
    /// <summary>
    /// Request used to iterate over the contents of a directory in a performant manner.
    /// </summary>
    public class IterateDirectoryContentsRequest : BaseSingleDirectoryRequest<IterateDirectoryContentsRequest>
    {
        /// <summary>
        /// Gets or sets a delegate to perform on each file or directory within the requested directory. The delegate's
        /// second parameter of type ExitIteration allows you to exit the loop at the end of the current iteration.
        /// </summary>
        public Func<PathRepresentation, ExitIteration, Task> Action { get; set; }

        /// <inheritdoc />
        internal override IterateDirectoryContentsRequest ShallowClone()
        {
            var cloned = base.ShallowClone();
            cloned.Action = Action;
            return cloned;
        }
    }
}
