using System;
using System.Collections.Generic;
using Baseline.Filesystem.Internal.Extensions;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Base class for managers that use stores to achieve their required goal.
    /// </summary>
    public abstract class BaseStoreWrapperManager
    {
        private readonly IStoreManager _storeManager;

        /// <summary>
        /// Initialises a new instance of the <see cref="BaseStoreWrapperManager" /> class.
        /// </summary>
        /// <param name="storeManager">
        /// An <see cref="IStoreManager" /> instance, used to retrieve adapters by name.
        /// </param>
        protected BaseStoreWrapperManager(IStoreManager storeManager)
        {
            _storeManager = storeManager;
        }

        /// <summary>
        /// Gets the root path for a store if it has one, or returns null.
        /// </summary>
        /// <param name="storeName">The name of the store to retrieve the root path from.</param>
        protected PathRepresentation GetStoreRootPath(string storeName)
        {
            return _storeManager.Get(storeName).RootPath;
        }

        /// <summary>
        /// Gets a store's registered adapter by its registered name.
        /// </summary>
        /// <param name="adapterName">The name the store is registered under.</param>
        protected IAdapter GetAdapter(string adapterName)
        {
            return _storeManager.Get(adapterName).Adapter;
        }

        /// <summary>
        /// Identifies and returns whether a store has a root path configured or not.
        /// </summary>
        /// <param name="storeName">The name of the store to check.</param>
        protected bool StoreHasRootPath(string storeName)
        {
            return GetStoreRootPath(storeName) != null;
        }
    }
}
