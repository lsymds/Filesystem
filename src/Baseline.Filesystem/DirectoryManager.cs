using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Baseline.Filesystem.Internal.Extensions;
using Baseline.Filesystem.Internal.Validators.Directories;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Provides a way to manage directories within a number of registered adapters.
    /// </summary>
    public class DirectoryManager : BaseAdapterWrapperManager, IDirectoryManager
    {
        /// <summary>
        /// Initialises a new <see cref="DirectoryManager" /> instance with all of its required dependencies.
        /// </summary>
        /// <param name="adapterManager">An adapter manager implementation.</param>
        public DirectoryManager(IAdapterManager adapterManager) : base(adapterManager)
        {
        }

        /// <inheritdoc />
        public async Task<CopyDirectoryResponse> CopyAsync(
            CopyDirectoryRequest copyDirectoryRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSourceAndDestinationDirectoryRequestValidator.ValidateAndThrowIfUnsuccessful(copyDirectoryRequest);

            return await GetAdapter(adapter)
                .CopyDirectoryAsync(
                    copyDirectoryRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)), 
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CreateDirectoryResponse> CreateAsync(
            CreateDirectoryRequest createDirectoryRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleDirectoryRequestValidator.ValidateAndThrowIfUnsuccessful(createDirectoryRequest);

            return await GetAdapter(adapter)
                .CreateDirectoryAsync(
                    createDirectoryRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<DeleteDirectoryResponse> DeleteAsync(
            DeleteDirectoryRequest deleteDirectoryRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleDirectoryRequestValidator.ValidateAndThrowIfUnsuccessful(deleteDirectoryRequest);
            
            return await GetAdapter(adapter)
                .DeleteDirectoryAsync(
                    deleteDirectoryRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)), 
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .ConfigureAwait(false);
        }
        
        /// <inheritdoc />
        public async Task<ListDirectoryContentsResponse> ListContentsAsync(
            ListDirectoryContentsRequest listDirectoryContentsRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleDirectoryRequestValidator.ValidateAndThrowIfUnsuccessful(listDirectoryContentsRequest);

            var response = await GetAdapter(adapter)
                .ListDirectoryContentsAsync(
                    listDirectoryContentsRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .ConfigureAwait(false);

            if (!AdapterHasRootPath(adapter))
            {
                return response;
            }

            return new ListDirectoryContentsResponse
            {
                Contents = response.Contents.RemoveRootPath(GetAdapterRootPath(adapter)).ToList()
            };
        }

        /// <inheritdoc />
        public async Task<MoveDirectoryResponse> MoveAsync(
            MoveDirectoryRequest moveDirectoryRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSourceAndDestinationDirectoryRequestValidator.ValidateAndThrowIfUnsuccessful(moveDirectoryRequest);
            
            return await GetAdapter(adapter)
                .MoveDirectoryAsync(
                    moveDirectoryRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .ConfigureAwait(false);
        }
    }
}
