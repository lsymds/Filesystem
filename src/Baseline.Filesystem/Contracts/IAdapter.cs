using System.Threading;
using System.Threading.Tasks;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Provides an interface for all Baseline.Filesystem adapters that back individual stores. Implementations are not
    /// aware that there may be more than one store within the parent store manager.
    /// </summary>
    public interface IAdapter
    {
        /// <summary>
        /// Copies a directory from one location in the store to another.
        /// </summary>
        /// <param name="copyDirectoryRequest">The request containing information about the directory to copy.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<CopyDirectoryResponse> CopyDirectoryAsync(
            CopyDirectoryRequest copyDirectoryRequest,
            CancellationToken cancellationToken
        );
        
        /// <summary>
        /// Copies a file from one location in the store to another.
        /// </summary>
        /// <param name="copyFileRequest">The request containing information about the file to copy.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<CopyFileResponse> CopyFileAsync(CopyFileRequest copyFileRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a directory in a store.
        /// </summary>
        /// <param name="createDirectoryRequest">
        /// The request containing information about the directory to create.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<CreateDirectoryResponse> CreateDirectoryAsync(
            CreateDirectoryRequest createDirectoryRequest,
            CancellationToken cancellationToken
        );

        /// <summary>
        /// Deletes a directory from a store.
        /// </summary>
        /// <param name="deleteDirectoryRequest">The request containing information about the directory to delete.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<DeleteDirectoryResponse> DeleteDirectoryAsync(
            DeleteDirectoryRequest deleteDirectoryRequest, 
            CancellationToken cancellationToken
        );
        
        /// <summary>
        /// Deletes a file from a store.
        /// </summary>
        /// <param name="deleteFileRequest">The request containing information about the file to delete.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<DeleteFileResponse> DeleteFileAsync(DeleteFileRequest deleteFileRequest, CancellationToken cancellationToken);
        
        /// <summary>
        /// Checks whether a file exists or not in the store.
        /// </summary>
        /// <param name="fileExistsRequest">The request containing information about the file to check.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<FileExistsResponse> FileExistsAsync(FileExistsRequest fileExistsRequest, CancellationToken cancellationToken);
        
        /// <summary>
        /// Gets a file's information (i.e. the name, path, extension, size etc) from the store.
        /// </summary>
        /// <param name="getFileRequest">The request containing information about the file to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<GetFileResponse> GetFileAsync(GetFileRequest getFileRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a public URL for a file from the store.
        /// </summary>
        /// <param name="getFilePublicUrlRequest">
        /// The request containing information about what file to retrieve a public URL for.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<GetFilePublicUrlResponse> GetFilePublicUrlAsync(
            GetFilePublicUrlRequest getFilePublicUrlRequest,
            CancellationToken cancellationToken
        );
        
        /// <summary>
        /// Iterates through a directory's contents within a store, executing a function for each directory and file
        /// in a recursive fashion.
        /// </summary>
        /// <param name="iterateDirectoryContentsRequest">
        /// The request which contains information about which directory to iterate through and which action to execute.
        /// </param>
        /// <param name="cancellationToken">A cancellation token.</param>
        Task<IterateDirectoryContentsResponse> IterateDirectoryContentsAsync(
            IterateDirectoryContentsRequest iterateDirectoryContentsRequest,
            CancellationToken cancellationToken
        );
        
        /// <summary>
        /// Lists the directory's contents from within the store.
        /// </summary>
        /// <param name="listDirectoryContentsRequest">
        /// The request which contains information about which directory to list the contents for.
        /// </param>
        /// <param name="cancellationToken">A cancellation token.</param>
        Task<ListDirectoryContentsResponse> ListDirectoryContentsAsync(
            ListDirectoryContentsRequest listDirectoryContentsRequest,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Moves a directory from one location in the store to another.
        /// </summary>
        /// <param name="moveDirectoryRequest">The request containing information about the directory to move.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<MoveDirectoryResponse> MoveDirectoryAsync(
            MoveDirectoryRequest moveDirectoryRequest,
            CancellationToken cancellationToken
        );

        /// <summary>
        /// Moves a file from one location in the store to another.
        /// </summary>
        /// <param name="moveFileRequest">
        /// The request containing information about the file to move and where to move it to.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<MoveFileResponse> MoveFileAsync(MoveFileRequest moveFileRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a file from the store and reads and returns its contents into a stream.
        /// </summary>
        /// <param name="readFileAsStreamRequest">
        /// The request containing information about the file to read the contents of.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token used to cancel asynchronous requests if required.
        /// </param>
        Task<ReadFileAsStreamResponse> ReadFileAsStreamAsync(
            ReadFileAsStreamRequest readFileAsStreamRequest,
            CancellationToken cancellationToken
        );

        /// <summary>
        /// Retrieves a file from the store and reads its contents as a string.
        /// </summary>
        /// <param name="readFileAsStringRequest">
        /// The request containing information about the file to read the contents of.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<ReadFileAsStringResponse> ReadFileAsStringAsync(
            ReadFileAsStringRequest readFileAsStringRequest,
            CancellationToken cancellationToken
        );
        
        /// <summary>
        /// Touches (creates without content) a file in the store.
        /// </summary>
        /// <param name="touchFileRequest">The request containing information about the file to touch.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<TouchFileResponse> TouchFileAsync(TouchFileRequest touchFileRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Writes a stream to a file within the store, creating it if it doesn't exist or overwriting it if it does.
        /// </summary>
        /// <param name="writeStreamToFileRequest">The request containing information about what to write and to where.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        Task<WriteStreamToFileResponse> WriteStreamToFileAsync(
            WriteStreamToFileRequest writeStreamToFileRequest,
            CancellationToken cancellationToken
        );

        /// <summary>
        /// Writes text to a file within the store, creating it if it doesn't exist or overwriting it if it does.
        /// </summary>
        /// <param name="writeTextToFileRequest">
        /// The request containing information about what to write and to where.
        /// </param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        Task<WriteTextToFileResponse> WriteTextToFileAsync(
            WriteTextToFileRequest writeTextToFileRequest, 
            CancellationToken cancellationToken
        );
    }
}
