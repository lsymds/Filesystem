using System.Threading.Tasks;

namespace Baseline.Filesystem.Internal.Extensions
{
    /// <summary>
    /// Extension methods related to the <see cref="DirectoryRepresentation" /> class. 
    /// </summary>
    public static class DirectoryRepresentationExtensions
    {
        /// <summary>
        /// Converts the <see cref="DirectoryRepresentation" /> into one that is adapter aware (i.e. it knows adapters
        /// exist).
        /// </summary>
        /// <param name="directoryRepresentation">The original, adapter ignorant directory representation.</param>
        /// <param name="adapterName">The adapter's name to associate with the directory representation.</param>
        /// <returns>The now adapter aware version of the directory representation passed in.</returns>
        public static AdapterAwareDirectoryRepresentation AsAdapterAwareRepresentation(
            this DirectoryRepresentation directoryRepresentation,
            string adapterName
        )
        {
            return new AdapterAwareDirectoryRepresentation
            {
                AdapterName = adapterName,
                Directory = directoryRepresentation
            };
        }

        /// <summary>
        /// Converts the awaitable task yielding a <see cref="DirectoryRepresentation" /> into one that is adapter aware
        /// (i.e. it knows adapters exist).
        /// </summary>
        /// <param name="directoryRepresentation">
        /// The awaitable task yielding the adapter ignorant directory representation.
        /// </param>
        /// <param name="adapterName">The adapter's name to associate with the directory representation.</param>
        /// <returns>The now adapter aware version of the directory representation passed in.</returns>
        public static async Task<AdapterAwareDirectoryRepresentation> AsAdapterAwareRepresentationAsync(
            this Task<DirectoryRepresentation> directoryRepresentation,
            string adapterName
        )
        {
            return (await directoryRepresentation).AsAdapterAwareRepresentation(adapterName);
        }
    }
}
