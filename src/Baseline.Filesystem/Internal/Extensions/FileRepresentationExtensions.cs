using System.Threading.Tasks;

namespace Baseline.Filesystem.Internal.Extensions
{
    /// <summary>
    /// Extension methods related to the <see cref="FileRepresentation" /> class. 
    /// </summary>
    internal static class FileRepresentationExtensions
    {
        /// <summary>
        /// Converts the <see cref="FileRepresentation" /> into one that is adapter aware (i.e. it knows adapters
        /// exist).
        /// </summary>
        /// <param name="fileRepresentation">The original, adapter ignorant file representation.</param>
        /// <param name="adapterName">The adapter's name to associate with the file representation.</param>
        /// <returns>The now adapter aware version of the file representation passed in.</returns>
        public static AdapterAwareFileRepresentation AsAdapterAwareRepresentation(
            this FileRepresentation fileRepresentation,
            string adapterName
        )
        {
            return new AdapterAwareFileRepresentation
            {
                AdapterName = adapterName,
                File = fileRepresentation
            };
        }

        /// <summary>
        /// Converts the awaitable task yielding a <see cref="FileRepresentation" /> into one that is adapter aware
        /// (i.e. it knows adapters exist).
        /// </summary>
        /// <param name="fileRepresentation">
        /// The awaitable task yielding the adapter ignorant file representation.
        /// </param>
        /// <param name="adapterName">The adapter's name to associate with the file representation.</param>
        /// <returns>The now adapter aware version of the file representation passed in.</returns>
        public static async Task<AdapterAwareFileRepresentation> AsAdapterAwareRepresentationAsync(
            this Task<FileRepresentation> fileRepresentation,
            string adapterName
        )
        {
            return (await fileRepresentation).AsAdapterAwareRepresentation(adapterName);
        }
    }
}
