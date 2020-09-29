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
        /// Gets whether or not the file defined by the request exists in the relevant adapter's file system.
        /// </summary>
        /// <param name="fileExistsRequest">
        /// The request containing information about the file to perform an existence check on.
        /// </param>
        /// <param name="adapter">The adapter to check the file's existence in.</param>
        /// <param name="cancellationToken">
        /// The cancellation token used to cancel asynchronous requests if required.
        /// </param>
        /// <returns>Whether or not the file defined by the request exists.</returns>
        Task<bool> ExistsAsync(
            FileExistsRequest fileExistsRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        );
        
        /// <summary>
        /// Gets a file's information from the relevant adapter's file system. This method does NOT retrieve the file's
        /// contents. Instead, you should use one of the more appropriate methods to do that.
        /// </summary>
        /// <param name="getFileRequest">The request containing information about the file to retrieve info for.</param>
        /// <param name="adapter">The adapter to get the file from.</param>
        /// <param name="cancellationToken">
        /// The cancellation token used to cancel asynchronous requests if required.
        /// </param>
        /// <returns>The file's information.</returns>
        Task<AdapterAwareFileRepresentation> GetAsync(
            GetFileRequest getFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        );
        
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

        /// <summary>
        /// Writes content to a path, overwriting it entirely if it already exists or creating it if it doesn't exist.
        /// </summary>
        /// <param name="writeTextToFileRequest">
        /// The request containing information about the file and content to write.
        /// </param>
        /// <param name="adapter">The adapter in which to write the file contents to.</param>
        /// <param name="cancellationToken">
        /// The cancellation token used to cancel asynchronous requests if required.
        /// </param>
        /// <returns>An awaitable task.</returns>
        Task WriteTextAsync(
            WriteTextToFileRequest writeTextToFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        );
    }
}
