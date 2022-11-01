using System.Threading;
using System.Threading.Tasks;

namespace Baseline.Filesystem;

/// <summary>
/// An <see cref="IAdapter"/> implementation for interacting with files and directories on a local disk (or one
/// masquerading as one).
/// </summary>
public partial class LocalAdapter
{
    /// <inheritdoc />
    public Task<CopyDirectoryResponse> CopyDirectoryAsync(
        CopyDirectoryRequest copyDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<CreateDirectoryResponse> CreateDirectoryAsync(
        CreateDirectoryRequest createDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<DeleteDirectoryResponse> DeleteDirectoryAsync(
        DeleteDirectoryRequest deleteDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<IterateDirectoryContentsResponse> IterateDirectoryContentsAsync(
        IterateDirectoryContentsRequest iterateDirectoryContentsRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ListDirectoryContentsResponse> ListDirectoryContentsAsync(
        ListDirectoryContentsRequest listDirectoryContentsRequest,
        CancellationToken cancellationToken = default
    )
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public Task<MoveDirectoryResponse> MoveDirectoryAsync(
        MoveDirectoryRequest moveDirectoryRequest,
        CancellationToken cancellationToken
    )
    {
        throw new System.NotImplementedException();
    }
}
