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
        /// Copies a folder from one location in the relevant adapter's file system to another location.
        /// </summary>
        /// <param name="copyFileRequest">
        /// The request containing information about the source file to copy from and where to copy it to.
        /// </param>
        /// <param name="adapter">The adapter to copy the file in.</param>
        /// <param name="cancellationToken">
        /// The cancellation token used to cancel asynchronous requests if required.
        /// </param>
        /// <returns>The adapter aware file representation of the newly copied file.</returns>
        Task<AdapterAwareFileRepresentation> CopyAsync(
            CopyFileRequest copyFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        );
        
        /// <summary>
        /// Deletes a file from the relevant adapter's file system.
        /// </summary>
        /// <param name="deleteFileRequest">The request containing information about the file to delete.</param>
        /// <param name="adapter">The adapter to delete the file from.</param>
        /// <param name="cancellationToken">
        /// The cancellation token used to cancel asynchronous requests if required.
        /// </param>
        /// <returns>An awaitable task.</returns>
        Task DeleteAsync(
            DeleteFileRequest deleteFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        );
        
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
        /// Moves a file from one path in an adapter's storage to another, returning the information about the newly
        /// created destination file.
        /// </summary>
        /// <param name="moveFileRequest">The requesting containing information about the file to move.</param>
        /// <param name="adapter">The adapter to move the file in.</param>
        /// <param name="cancellationToken">
        /// The cancellation token used to cancel asynchronous requests if required.
        /// </param>
        /// <returns>The newly created destination file's information.</returns>
        Task<AdapterAwareFileRepresentation> MoveAsync(
            MoveFileRequest moveFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Reads a file's contents into a string.
        /// </summary>
        /// <param name="readFileAsStringRequest">
        /// The request containing information about the file to read the contents of.
        /// </param>
        /// <param name="adapter">The adapter where the file to read the contents of is stored.</param>
        /// <param name="cancellationToken">
        /// The cancellation token used to cancel asynchronous requests if required.
        /// </param>
        /// <returns>The file's contents.</returns>
        Task<string> ReadAsStringAsync(
            ReadFileAsStringRequest readFileAsStringRequest,
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
