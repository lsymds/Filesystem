using System.Threading;
using System.Threading.Tasks;

namespace Storio
{
    /// <summary>
    /// Provides an interface for all file management operations.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Touches (creates without content) a file in the relevant adapter's file system.
        /// </summary>
        /// <param name="touchFileRequest">The request containing information about the file to create.</param>
        /// <param name="adapter">The adapter in which to touch the file.</param>
        /// <param name="cancellationToken">
        /// The cancellation token used to cancel asynchronous requests if required.
        /// </param>
        /// <returns>The created file's information.</returns>
        Task<AdapterAwareFileRepresentation> TouchAsync(
            TouchFileRequest touchFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        );
    }
}
