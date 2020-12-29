using System.Collections.Concurrent;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Provides a way to manage adapters.
    /// </summary>
    public class AdapterManager : IAdapterManager
    {
        private readonly ConcurrentDictionary<string, IAdapter> _adapters =
            new ConcurrentDictionary<string, IAdapter>();
            
        /// <inheritdoc />
        public IAdapter Get(string name)
        {
            var normalisedName = NormaliseAdapterName(name);

            if (!AdapterAlreadyRegistered(normalisedName))
                throw new AdapterNotFoundException(name);
            
            return _adapters[normalisedName];
        }

        /// <inheritdoc />
        public void Register(IAdapter adapter, string name = "default")
        {
            var normalisedName = NormaliseAdapterName(name);

            if (AdapterAlreadyRegistered(normalisedName))
                throw new AdapterAlreadyRegisteredException(normalisedName);

            _adapters[normalisedName] = adapter;
        }

        /// <summary>
        /// Normalises a given adapter name to ensure all adapter names are formatted in the same way. Currently,
        /// adapter names are simply lowercased.
        /// </summary>
        /// <param name="name">The original adapter name.</param>
        /// <returns>The normalised adapter name.</returns>
        private string NormaliseAdapterName(string name)
        {
            return name.ToLower();
        }

        /// <summary>
        /// Checks whether an adapter with the given, normalised name has already been registered.
        /// </summary>
        /// <param name="name">The normalised adapter name to check.</param>
        /// <returns>Whether the normalised name has already been registered.</returns>
        private bool AdapterAlreadyRegistered(string name)
        {
            return _adapters.ContainsKey(name);
        }
    }
}
