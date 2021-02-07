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
        public Task<AdapterAwareFileRepresentation> CopyAsync(
            CopyFileRequest copyFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSourceAndDestinationFileRequestValidator.ValidateAndThrowIfUnsuccessful(copyFileRequest);
            
            return GetAdapter(adapter)
                .CopyFileAsync(
                    copyFileRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .AsAdapterAwareRepresentationAsync(adapter);
        }

        /// <inheritdoc />
        public Task DeleteAsync(
            DeleteFileRequest deleteFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(deleteFileRequest);

            return GetAdapter(adapter)
                .DeleteFileAsync(
                    deleteFileRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter);
        }

        /// <inheritdoc />
        public Task<bool> ExistsAsync(
            FileExistsRequest fileExistsRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(fileExistsRequest);

            return GetAdapter(adapter)
                .FileExistsAsync(
                    fileExistsRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)), 
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter);
        }

        /// <inheritdoc />
        public Task<AdapterAwareFileRepresentation> GetAsync(
            GetFileRequest getFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(getFileRequest);
    
            return GetAdapter(adapter)
                .GetFileAsync(
                    getFileRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .AsAdapterAwareRepresentationAsync(adapter);
        }

        /// <inheritdoc />
        public Task<AdapterAwareFileRepresentation> MoveAsync(
            MoveFileRequest moveFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSourceAndDestinationFileRequestValidator.ValidateAndThrowIfUnsuccessful(moveFileRequest);

            return GetAdapter(adapter)
                .MoveFileAsync(
                    moveFileRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .AsAdapterAwareRepresentationAsync(adapter);
        }

        /// <inheritdoc />
        public Task<string> ReadAsStringAsync(
            ReadFileAsStringRequest readFileAsStringRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(readFileAsStringRequest);

            return GetAdapter(adapter)
                .ReadFileAsStringAsync(
                    readFileAsStringRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter);
        }

        /// <inheritdoc />
        public Task<AdapterAwareFileRepresentation> TouchAsync(
            TouchFileRequest touchFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseSingleFileRequestValidator.ValidateAndThrowIfUnsuccessful(touchFileRequest);
            
            return GetAdapter(adapter)
                .TouchFileAsync(
                    touchFileRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter)
                .AsAdapterAwareRepresentationAsync(adapter);
        }

        /// <inheritdoc />
        public Task WriteTextAsync(
            WriteTextToFileRequest writeTextToFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            WriteTextToFileRequestValidator.ValidateAndThrowIfUnsuccessful(writeTextToFileRequest);

            return GetAdapter(adapter)
                .WriteTextToFileAsync(
                    writeTextToFileRequest.CloneAndCombinePathsWithRootPath(GetAdapterRootPath(adapter)),
                    cancellationToken
                )
                .WrapExternalExceptionsAsync(adapter);
        }
    }
}
