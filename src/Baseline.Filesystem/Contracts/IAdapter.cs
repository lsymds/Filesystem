using System.Threading;
using System.Threading.Tasks;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Provides an interface for all Baseline.Filesystem adapters. Implementations are not aware that there may be more than one
    /// adapter within the parent adapter manager.
    /// </summary>
    public interface IAdapter
    {
        /// <summary>
        /// Copies a directory from one location in the adapter's data store to another.
        /// </summary>
        /// <param name="copyDirectoryRequest">The request containing information about the directory to copy.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        /// <returns>The information about the directory that was created (the destination directory).</returns>
        Task<DirectoryRepresentation> CopyDirectoryAsync(
            CopyDirectoryRequest copyDirectoryRequest,
            CancellationToken cancellationToken
        );
        
        /// <summary>
        /// Copies a file from one location in the adapter's data store to another.
        /// </summary>
        /// <param name="copyFileRequest">The request containing information about the file to copy.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        /// <returns>The information about the file that was created (the destination file).</returns>
        Task<FileRepresentation> CopyFileAsync(CopyFileRequest copyFileRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a directory in an adapter's data store.
        /// </summary>
        /// <param name="createDirectoryRequest">
        /// The request containing information about the directory to create.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        /// <returns>The information about the directory that was created (the destination directory).</returns>
        Task<DirectoryRepresentation> CreateDirectoryAsync(
            CreateDirectoryRequest createDirectoryRequest,
            CancellationToken cancellationToken
        );

        /// <summary>
        /// Deletes a directory from an adapter's data store.
        /// </summary>
        /// <param name="deleteDirectoryRequest">The request containing information about the directory to delete.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task DeleteDirectoryAsync(DeleteDirectoryRequest deleteDirectoryRequest, CancellationToken cancellationToken);
        
        /// <summary>
        /// Deletes a file from the adapter's data store.
        /// </summary>
        /// <param name="deleteFileRequest">The request containing information about the file to delete.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task DeleteFileAsync(DeleteFileRequest deleteFileRequest, CancellationToken cancellationToken);
        
        /// <summary>
        /// Checks whether a file exists or not in the adapter's data store.
        /// </summary>
        /// <param name="fileExistsRequest">The request containing information about the file to check.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        /// <returns>Whether or not the file defined in the request exists.</returns>
        Task<bool> FileExistsAsync(FileExistsRequest fileExistsRequest, CancellationToken cancellationToken);
        
        /// <summary>
        /// Gets a file's information (i.e. the name, path, extension, size etc) from the adapter's data store.
        /// </summary>
        /// <param name="getFileRequest">The request containing information about the file to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        /// <returns>The information about the requested file.</returns>
        Task<FileRepresentation> GetFileAsync(GetFileRequest getFileRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Moves a directory from one location in the adapter's data store to another.
        /// </summary>
        /// <param name="moveDirectoryRequest">The request containing information about the directory to move.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        /// <returns>The information about the directory that was created (the destination directory).</returns>
        Task<DirectoryRepresentation> MoveDirectoryAsync(
            MoveDirectoryRequest moveDirectoryRequest,
            CancellationToken cancellationToken
        );

        /// <summary>
        /// Moves a file from one location in the adapter's data store to another.
        /// </summary>
        /// <param name="moveFileRequest">
        /// The request containing information about the file to move and where to move it to.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        /// <returns>The information about the destination file of the move request.</returns>
        Task<FileRepresentation> MoveFileAsync(MoveFileRequest moveFileRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a file from the adapter's data store and reads its contents as a string.
        /// </summary>
        /// <param name="readFileAsStringRequest">
        /// The request containing information about the file to read the contents of.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        /// <returns>The file's contents.</returns>
        Task<string> ReadFileAsStringAsync(
            ReadFileAsStringRequest readFileAsStringRequest,
            CancellationToken cancellationToken
        );
        
        /// <summary>
        /// Touches (creates without content) a file in the adapter's data store.
        /// </summary>
        /// <param name="touchFileRequest">The request containing information about the file to touch.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        /// <returns>The information about the file that was created.</returns>
        Task<FileRepresentation> TouchFileAsync(TouchFileRequest touchFileRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Writes text to a file within the adapter's data store, creating it if it doesn't exist or overwriting it if
        /// it does.
        /// </summary>
        /// <param name="writeTextToFileRequest">
        /// The request containing information about what to write and to where.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        /// <returns>An asynchronous task.</returns>
        Task WriteTextToFileAsync(WriteTextToFileRequest writeTextToFileRequest, CancellationToken cancellationToken);
    }
}
