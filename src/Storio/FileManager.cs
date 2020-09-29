using System.Threading;
using System.Threading.Tasks;
using Storio.Internal.Extensions;
using Storio.Internal.Validators;

namespace Storio
{
    /// <summary>
    /// Provides a way to manage files within a number of registered adapters.
    /// </summary>
    public class FileManager : IFileManager
    {
        private readonly IAdapterManager _adapterManager;

        /// <summary>
        /// Initialises a new <see cref="FileManager" /> instance with all of its required dependencies.
        /// </summary>
        /// <param name="adapterManager">An adapter manager implementation.</param>
        public FileManager(IAdapterManager adapterManager)
        {
            _adapterManager = adapterManager;
        }

        /// <inheritdoc />
        public Task DeleteAsync(
            DeleteFileRequest deleteFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseFileRequestValidator.ValidateAndThrowIfUnsuccessful(deleteFileRequest);

            return _adapterManager
                .Get(adapter)
                .DeleteFileAsync(deleteFileRequest, cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> ExistsAsync(
            FileExistsRequest fileExistsRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseFileRequestValidator.ValidateAndThrowIfUnsuccessful(fileExistsRequest);

            return _adapterManager
                .Get(adapter)
                .FileExistsAsync(fileExistsRequest, cancellationToken);
        }

        /// <inheritdoc />
        public Task<AdapterAwareFileRepresentation> GetAsync(
            GetFileRequest getFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseFileRequestValidator.ValidateAndThrowIfUnsuccessful(getFileRequest);
    
            return _adapterManager
                .Get(adapter)
                .GetFileAsync(getFileRequest, cancellationToken)
                .AsAdapterAwareRepresentation(adapter);
        }

        /// <inheritdoc />
        public Task<AdapterAwareFileRepresentation> TouchAsync(
            TouchFileRequest touchFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            BaseFileRequestValidator.ValidateAndThrowIfUnsuccessful(touchFileRequest);
            
            return _adapterManager
                .Get(adapter)
                .TouchFileAsync(touchFileRequest, cancellationToken)
                .AsAdapterAwareRepresentation(adapter);
        }

        /// <inheritdoc />
        public Task WriteTextAsync(
            WriteTextToFileRequest writeTextToFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            WriteTextToFileRequestValidator.ValidateAndThrowIfUnsuccessful(writeTextToFileRequest);

            return _adapterManager
                .Get(adapter)
                .WriteTextToFileAsync(writeTextToFileRequest, cancellationToken);
        }
    }
}
