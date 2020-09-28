using System.Threading;
using System.Threading.Tasks;

namespace Storio
{
    /// <summary>
    /// Provides an interface for all Storio adapters. Implementations are not aware that there may be more than one
    /// adapter within the parent adapter manager.
    /// </summary>
    public interface IAdapter
    {
        /// <summary>
        /// Touches (creates without content) a file in the adapter's data store.
        /// </summary>
        /// <param name="touchFileRequest">The request containing information about the file to touch.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel any asynchronous tasks.</param>
        /// <returns>The information about the file that was created.</returns>
        Task<FileRepresentation> TouchFileAsync(TouchFileRequest touchFileRequest, CancellationToken cancellationToken);
    }
}
