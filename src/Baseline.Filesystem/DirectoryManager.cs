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
                .RemoveRootPathsAsync(
                    x => x.DestinationDirectory.Path, 
                    (o, p) => o.DestinationDirectory.Path = p, 
                    GetAdapterRootPath(adapter)
                )
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
                .RemoveRootPathsAsync(
                    r => r.Directory.Path,
                    (r, p) => r.Directory.Path = p,
                    GetAdapterRootPath(adapter)
                )
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
        public async Task<IterateDirectoryContentsResponse> IterateContentsAsync(
            IterateDirectoryContentsRequest iterateDirectoryContentsRequest, 
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            IterateDirectoryContentsRequestValidator.ValidateAndThrowIfUnsuccessful(iterateDirectoryContentsRequest);

            var request = new IterateDirectoryContentsRequest
            {
                DirectoryPath = iterateDirectoryContentsRequest.DirectoryPath,
                Action = async path =>
                {
                    if (!AdapterHasRootPath(adapter))
                    {
                        await iterateDirectoryContentsRequest.Action(path);
                        return;
                    }
                    
                    var pathWithoutRoot = new[] {path}.RemoveRootPath(GetAdapterRootPath(adapter)).ToList();
                    if (pathWithoutRoot.Any())
                    {
                        await iterateDirectoryContentsRequest.Action(pathWithoutRoot.First());   
                    }
                }
            };

            return await GetAdapter(adapter)
                .IterateDirectoryContentsAsync(
                    request.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
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

            return await GetAdapter(adapter)
                .ListDirectoryContentsAsync(
                    listDirectoryContentsRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .RemoveRootPathsAsync(
                    r => r.Contents,
                    (ri, p) => ri.Contents = p.ToList(),
                    GetAdapterRootPath(adapter)
                )
                .ConfigureAwait(false);
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
                .RemoveRootPathsAsync(
                    r => r.DestinationDirectory.Path,
                    (r, p) => r.DestinationDirectory.Path = p,
                    GetAdapterRootPath(adapter)
                )
                .ConfigureAwait(false);
        }
    }
}
