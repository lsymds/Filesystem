using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baseline.Filesystem
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
        /// <returns>The file representation of the newly copied file.</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="AdapterNotFoundException" />
        /// <exception cref="AdapterProviderOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsADirectoryException" />
        /// <exception cref="FileNotFoundException" />
        /// <exception cref="FileAlreadyExistsException" />
        Task<FileRepresentation> CopyAsync(
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
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="AdapterNotFoundException" />
        /// <exception cref="AdapterProviderOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsADirectoryException" />
        /// <exception cref="FileNotFoundException" />
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
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="AdapterNotFoundException" />
        /// <exception cref="AdapterProviderOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsADirectoryException" />
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
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="AdapterNotFoundException" />
        /// <exception cref="AdapterProviderOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsADirectoryException" />
        /// <exception cref="FileNotFoundException" />
        Task<FileRepresentation> GetAsync(
            GetFileRequest getFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        );
        
        /// <summary>
        /// Retrieves a publicly accessible URL for the file defined in the request.
        /// </summary>
        /// <param name="getFilePublicUrlRequest">
        /// The request containing the information about the file to get the public URL for.
        /// </param>
        /// <param name="adapter">The adapter to get the public URL for the file from.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A response containing the public URL and related information of the file.</returns>
        Task<GetFilePublicUrlResponse> GetPublicUrlAsync(
            GetFilePublicUrlRequest getFilePublicUrlRequest,
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
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="AdapterNotFoundException" />
        /// <exception cref="AdapterProviderOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsADirectoryException" />
        /// <exception cref="FileNotFoundException" />
        /// <exception cref="FileAlreadyExistsException" />
        Task<FileRepresentation> MoveAsync(
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
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="AdapterNotFoundException" />
        /// <exception cref="AdapterProviderOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsADirectoryException" />
        /// <exception cref="FileNotFoundException" />
        Task<string> ReadAsStringAsync(
            ReadFileAsStringRequest readFileAsStringRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        );
        
        /// <summary>
        /// Touches (creates without content) a file in the relevant adapter's file system. BEWARE: In some adapters
        /// this will overwrite your files without throwing any exception about the file existing. In others, it will.
        /// We're aiming to normalise this behavior in a future release.
        /// </summary>
        /// <param name="touchFileRequest">The request containing information about the file to create.</param>
        /// <param name="adapter">The adapter in which to touch the file.</param>
        /// <param name="cancellationToken">
        /// The cancellation token used to cancel asynchronous requests if required.
        /// </param>
        /// <returns>The created file's information.</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="AdapterNotFoundException" />
        /// <exception cref="AdapterProviderOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsADirectoryException" />
        /// <exception cref="FileAlreadyExistsException" />
        Task<FileRepresentation> TouchAsync(
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
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="AdapterNotFoundException" />
        /// <exception cref="AdapterProviderOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsADirectoryException" />
        Task WriteTextAsync(
            WriteTextToFileRequest writeTextToFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Writes a stream to a path, overwriting it entirely if it already exists or creating it if it doesn't.
        /// </summary>
        /// <param name="writeStreamToFileRequest">
        /// The request containing information about the file and content to write.
        /// </param>
        /// <param name="adapter">The adapter in which to write the file stream to.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A response containing information about what was created.</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="AdapterNotFoundException" />
        /// <exception cref="AdapterProviderOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsADirectoryException" />
        Task<WriteStreamToFileResponse> WriteStreamAsync(
            WriteStreamToFileRequest writeStreamToFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        );
    }
}
