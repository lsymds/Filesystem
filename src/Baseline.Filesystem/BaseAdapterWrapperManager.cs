namespace Baseline.Filesystem
{
    /// <summary>
    /// Base class for managers that use adapters to achieve their required goal.
    /// </summary>
    public abstract class BaseAdapterWrapperManager
    {
        private readonly IAdapterManager _adapterManager;

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseAdapterWrapperManager" /> class.
        /// </summary>
        /// <param name="adapterManager">
        /// An <see cref="IAdapterManager" /> instance, used to retrieve adapters by name.
        /// </param>
        protected BaseAdapterWrapperManager(IAdapterManager adapterManager)
        {
            _adapterManager = adapterManager;
        }

        /// <summary>
        /// Gets the root path for an adapter if it has one, or returns null.
        /// </summary>
        /// <param name="adapterName">The name of the adapter to retrieve the root path from.</param>
        /// <returns>The root path for the adapter if it has one.</returns>
        protected PathRepresentation GetAdapterRootPath(string adapterName)
        {
            return _adapterManager.Get(adapterName).RootPath;
        }

        /// <summary>
        /// Gets an adapter by its registered name.
        /// </summary>
        /// <param name="adapterName">The name the adapter is registered under.</param>
        /// <returns>The adapter, if it is registered, or an exception if it is not.</returns>
        protected IAdapter GetAdapter(string adapterName)
        {
            return _adapterManager.Get(adapterName).Adapter;
        }
    }
}
