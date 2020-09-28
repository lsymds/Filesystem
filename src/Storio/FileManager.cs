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
        public Task<AdapterAwareFileRepresentation> TouchAsync(
            TouchFileRequest touchFileRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            TouchFileRequestValidator.ValidateAndThrowIfUnsuccessful(touchFileRequest);
            
            return _adapterManager
                .Get(adapter)
                .TouchFileAsync(touchFileRequest, cancellationToken)
                .AsAdapterAwareRepresentation(adapter);
        }
    }
}
