namespace Storio
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
        /// Gets an adapter from the adapter manage by its registered name.
        /// </summary>
        /// <param name="adapterName">The name to retrieve the adapter by.</param>
        /// <returns>The adapter retrieved from the adapter manager.</returns>
        protected IAdapter GetAdapter(string adapterName)
        {
            return _adapterManager.Get(adapterName);
        }
    }
}
