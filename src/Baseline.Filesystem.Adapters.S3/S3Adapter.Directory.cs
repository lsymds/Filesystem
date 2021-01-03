using System.Threading;
using System.Threading.Tasks;

namespace Baseline.Filesystem.Adapters.S3
{
    /// <summary>
    /// Provides the directory based functions of an <see cref="IAdapter"/> for Amazon's Simple Storage Service. 
    /// </summary>
    public partial class S3Adapter
    {
        /// <inheritdoc />
        public Task<DirectoryRepresentation> CopyDirectoryAsync(
            CopyDirectoryRequest copyDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<DirectoryRepresentation> CreateDirectoryAsync(
            CreateDirectoryRequest createDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task DeleteDirectoryAsync(
            DeleteDirectoryRequest deleteDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<DirectoryRepresentation> MoveDirectoryAsync(
            MoveDirectoryRequest moveDirectoryRequest,
            CancellationToken cancellationToken
        )
        {
            throw new System.NotImplementedException();
        }
    }
}
