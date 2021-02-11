using System.Threading;
using System.Threading.Tasks;
using Baseline.Filesystem.Internal.Extensions;
using Baseline.Filesystem.Internal.Validators.Files;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Provides a way to manage files within a number of registered adapters.
    /// </summary>
    public class FileManager : BaseAdapterWrapperManager, IFileManager
    {
        /// <summary>
        /// Initialises a new <see cref="FileManager" /> instance with all of its required dependencies.
        /// </summary>
        /// <param name="adapterManager">An adapter manager implementation.</param>
        public FileManager(IAdapterManager adapterManager) : base(adapterManager)
        {
        }

        /// <inheritdoc />
        public async Task<AdapterAwareFileRepresentation> CopyAsync(
            CopyFileRequest copyFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSourceAndDestinationFileRequestValidator.ValidateAndThrowIfUnsuccessful(copyFileRequest);
            
            return await GetAdapter(adapter)
                .CopyFileAsync(
                    copyFileRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .AsAdapterAwareRepresentationAsync(adapter)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(
            DeleteFileRequest deleteFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(deleteFileRequest);

            await GetAdapter(adapter)
                .DeleteFileAsync(
                    deleteFileRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(
            FileExistsRequest fileExistsRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(fileExistsRequest);

            return await GetAdapter(adapter)
                .FileExistsAsync(
                    fileExistsRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)), 
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<AdapterAwareFileRepresentation> GetAsync(
            GetFileRequest getFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(getFileRequest);
    
            return await GetAdapter(adapter)
                .GetFileAsync(
                    getFileRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .AsAdapterAwareRepresentationAsync(adapter)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<GetFilePublicUrlResponse> GetPublicUrlAsync(
            GetFilePublicUrlRequest getFilePublicUrlRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            GetFilePublicUrlRequestValidator.ValidateAndThrowIfUnsuccessful(getFilePublicUrlRequest);

            return await GetAdapter(adapter)
                .GetFilePublicUrlAsync(
                    getFilePublicUrlRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)), 
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<AdapterAwareFileRepresentation> MoveAsync(
            MoveFileRequest moveFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSourceAndDestinationFileRequestValidator.ValidateAndThrowIfUnsuccessful(moveFileRequest);

            return await GetAdapter(adapter)
                .MoveFileAsync(
                    moveFileRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .AsAdapterAwareRepresentationAsync(adapter)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<string> ReadAsStringAsync(
            ReadFileAsStringRequest readFileAsStringRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(readFileAsStringRequest);

            return await GetAdapter(adapter)
                .ReadFileAsStringAsync(
                    readFileAsStringRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<AdapterAwareFileRepresentation> TouchAsync(
            TouchFileRequest touchFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(touchFileRequest);
            
            return await GetAdapter(adapter)
                .TouchFileAsync(
                    touchFileRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .AsAdapterAwareRepresentationAsync(adapter)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task WriteTextAsync(
            WriteTextToFileRequest writeTextToFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            WriteTextToFileRequestValidator.ValidateAndThrowIfUnsuccessful(writeTextToFileRequest);

            
            await GetAdapter(adapter)
                .WriteTextToFileAsync(
                    writeTextToFileRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .ConfigureAwait(false);
        }
    }
}
