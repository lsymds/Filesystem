using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Provides an interface for all directory management operations.
    /// </summary>
    public interface IDirectoryManager
    {
        /// <summary>
        /// Copies a directory from one location to another. Please note: some adapters require each file within the
        /// directory tree to be copied in order to copy a directory and all of its contained files. This could have
        /// unintended performance consequences, so be careful in its use. Consider doing this manually.
        /// </summary>
        /// <param name="copyDirectoryRequest">
        /// The request which contains information about the directory to copy.
        /// </param>
        /// <param name="store">The store in which to perform the action.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The representation of the directory the requested source directory was copied to.</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="StoreNotFoundException" />
        /// <exception cref="StoreAdapterOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsNotObviouslyADirectoryException" />
        /// <exception cref="DirectoryNotFoundException" />
        /// <exception cref="DirectoryAlreadyExistsException" />
        Task<CopyDirectoryResponse> CopyAsync(
            CopyDirectoryRequest copyDirectoryRequest,
            string store = "default",
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Creates a directory within the chosen store.
        /// </summary>
        /// <param name="createDirectoryRequest">
        /// The request which contains information about the directory to copy.
        /// </param>
        /// <param name="store">The store in which to perform the action.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The representation of the directory that was created.</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="StoreNotFoundException" />
        /// <exception cref="StoreAdapterOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsNotObviouslyADirectoryException" />
        /// <exception cref="DirectoryAlreadyExistsException" />
        Task<CreateDirectoryResponse> CreateAsync(
            CreateDirectoryRequest createDirectoryRequest,
            string store = "default",
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Deletes a directory within the chosen store. Please note: some adapters require each file within the
        /// directory tree to be deleted in order to delete a directory and all of its contained files. This could have
        /// unintended performance consequences, so be careful in its use. Consider doing this manually, and/or read
        /// the documentation for the adapter ensuring you understand the performance implications.
        /// </summary>
        /// <param name="deleteDirectoryRequest">
        /// The request which contains information about the directory to delete.
        /// </param>
        /// <param name="store">The store in which to perform the action.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="StoreNotFoundException" />
        /// <exception cref="StoreAdapterOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsNotObviouslyADirectoryException" />
        /// <exception cref="DirectoryNotFoundException" />
        Task<DeleteDirectoryResponse> DeleteAsync(
            DeleteDirectoryRequest deleteDirectoryRequest,
            string store = "default",
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Iterates through a directory's contents, executing a function for each directory and file in a recursive
        /// fashion.
        /// </summary>
        /// <param name="iterateDirectoryContentsRequest">
        /// The request which contains information about which directory to iterate through and which action to execute.
        /// </param>
        /// <param name="store">The store to run the list operation against.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="StoreNotFoundException" />
        /// <exception cref="StoreAdapterOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsNotObviouslyADirectoryException" />
        /// <exception cref="DirectoryNotFoundException" />
        Task<IterateDirectoryContentsResponse> IterateContentsAsync(
            IterateDirectoryContentsRequest iterateDirectoryContentsRequest,
            string store = "default",
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Lists the directory's contents, returning the available paths as a flat list. Directories will be
        /// returned in the list prior to any child files/directories.
        /// </summary>
        /// <param name="listDirectoryContentsRequest">
        /// The request which contains information about which directory to list the contents for.
        /// </param>
        /// <param name="store">The store to run the list operation against.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A response containing the available paths in the directory.</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="StoreNotFoundException" />
        /// <exception cref="StoreAdapterOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsNotObviouslyADirectoryException" />
        /// <exception cref="DirectoryNotFoundException" />
        Task<ListDirectoryContentsResponse> ListContentsAsync(
            ListDirectoryContentsRequest listDirectoryContentsRequest,
            string store = "default",
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Moves a directory from one location to another. Please note: some adapters require each file within the
        /// directory tree to be moved in order to move a directory and all of its contained files. This could have
        /// unintended performance consequences, so be careful in its use. Consider doing this manually and/or read
        /// the documentation for the adapter to ensure you understand the performance implications.
        /// </summary>
        /// <param name="moveDirectoryRequest">
        /// The request which contains information about the directory to move.
        /// </param>
        /// <param name="store">The store in which to perform the action.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The representation of the directory the requested source directory was moved to.</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="StoreNotFoundException" />
        /// <exception cref="StoreAdapterOperationException" />
        /// <exception cref="PathIsBlankException" />
        /// <exception cref="PathContainsInvalidCharacterException" />
        /// <exception cref="PathIsRelativeException" />
        /// <exception cref="PathIsNotObviouslyADirectoryException" />
        /// <exception cref="DirectoryNotFoundException" />
        /// <exception cref="DirectoryAlreadyExistsException" />
        Task<MoveDirectoryResponse> MoveAsync(
            MoveDirectoryRequest moveDirectoryRequest,
            string store = "default",
            CancellationToken cancellationToken = default
        );
    }
}
