using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Baseline.Filesystem.Internal.Extensions;
using Baseline.Filesystem.Internal.Validators.Directories;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Provides a way to manage directories within a number of registered stores.
    /// </summary>
    public class DirectoryManager : BaseStoreWrapperManager, IDirectoryManager
    {
        /// <summary>
        /// Initialises a new <see cref="DirectoryManager" /> instance with all of its required dependencies.
        /// </summary>
        /// <param name="storeManager">A store manager implementation.</param>
        public DirectoryManager(IStoreManager storeManager) : base(storeManager)
        {
        }

        /// <inheritdoc />
        public async Task<CopyDirectoryResponse> CopyAsync(
            CopyDirectoryRequest copyDirectoryRequest,
            string store = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSourceAndDestinationDirectoryRequestValidator.ValidateAndThrowIfUnsuccessful(copyDirectoryRequest);

            return await GetAdapter(store)
                .CopyDirectoryAsync(
                    copyDirectoryRequest.CloneAndCombinePathsWithRootPath(GetStoreRootPath(store)), 
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(store)
                .RemoveRootPathsAsync(
                    x => x.DestinationDirectory.Path, 
                    (o, p) => o.DestinationDirectory.Path = p, 
                    GetStoreRootPath(store)
                )
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CreateDirectoryResponse> CreateAsync(
            CreateDirectoryRequest createDirectoryRequest,
            string store = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleDirectoryRequestValidator.ValidateAndThrowIfUnsuccessful(createDirectoryRequest);

            return await GetAdapter(store)
                .CreateDirectoryAsync(
                    createDirectoryRequest.CloneAndCombinePathsWithRootPath(GetStoreRootPath(store)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(store)
                .RemoveRootPathsAsync(
                    r => r.Directory.Path,
                    (r, p) => r.Directory.Path = p,
                    GetStoreRootPath(store)
                )
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<DeleteDirectoryResponse> DeleteAsync(
            DeleteDirectoryRequest deleteDirectoryRequest,
            string store = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleDirectoryRequestValidator.ValidateAndThrowIfUnsuccessful(deleteDirectoryRequest);
            
            return await GetAdapter(store)
                .DeleteDirectoryAsync(
                    deleteDirectoryRequest.CloneAndCombinePathsWithRootPath(GetStoreRootPath(store)), 
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(store)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IterateDirectoryContentsResponse> IterateContentsAsync(
            IterateDirectoryContentsRequest iterateDirectoryContentsRequest, 
            string store = "default",
            CancellationToken cancellationToken = default
        )
        {
            IterateDirectoryContentsRequestValidator.ValidateAndThrowIfUnsuccessful(iterateDirectoryContentsRequest);

            var request = new IterateDirectoryContentsRequest
            {
                DirectoryPath = iterateDirectoryContentsRequest.DirectoryPath,
                Action = async path =>
                {
                    if (!StoreHasRootPath(store))
                    {
                        await iterateDirectoryContentsRequest.Action(path);
                        return;
                    }
                    
                    var pathWithoutRoot = new[] {path}.RemoveRootPath(GetStoreRootPath(store)).ToList();
                    if (pathWithoutRoot.Any())
                    {
                        await iterateDirectoryContentsRequest.Action(pathWithoutRoot.First());   
                    }
                }
            };

            return await GetAdapter(store)
                .IterateDirectoryContentsAsync(
                    request.CloneAndCombinePathsWithRootPath(GetStoreRootPath(store)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(store)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<ListDirectoryContentsResponse> ListContentsAsync(
            ListDirectoryContentsRequest listDirectoryContentsRequest,
            string store = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleDirectoryRequestValidator.ValidateAndThrowIfUnsuccessful(listDirectoryContentsRequest);

            return await GetAdapter(store)
                .ListDirectoryContentsAsync(
                    listDirectoryContentsRequest.CloneAndCombinePathsWithRootPath(GetStoreRootPath(store)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(store)
                .RemoveRootPathsAsync(
                    r => r.Contents,
                    (ri, p) => ri.Contents = p.ToList(),
                    GetStoreRootPath(store)
                )
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<MoveDirectoryResponse> MoveAsync(
            MoveDirectoryRequest moveDirectoryRequest,
            string store = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSourceAndDestinationDirectoryRequestValidator.ValidateAndThrowIfUnsuccessful(moveDirectoryRequest);
            
            return await GetAdapter(store)
                .MoveDirectoryAsync(
                    moveDirectoryRequest.CloneAndCombinePathsWithRootPath(GetStoreRootPath(store)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(store)
                .RemoveRootPathsAsync(
                    r => r.DestinationDirectory.Path,
                    (r, p) => r.DestinationDirectory.Path = p,
                    GetStoreRootPath(store)
                )
                .ConfigureAwait(false);
        }
    }
}
