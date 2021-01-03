using System.Threading;
using System.Threading.Tasks;

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
        public Task<AdapterAwareDirectoryRepresentation> CopyAsync(
            CopyDirectoryRequest copyDirectoryRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<AdapterAwareDirectoryRepresentation> CreateAsync(
            CreateDirectoryRequest createDirectoryRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task DeleteAsync(
            DeleteDirectoryRequest deleteDirectoryRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<AdapterAwareDirectoryRepresentation> MoveAsync(
            MoveDirectoryRequest moveDirectoryRequest,
            string adapter = "default",
            CancellationToken cancellationToken = default
        )
        {
            throw new System.NotImplementedException();
        }
    }
}
