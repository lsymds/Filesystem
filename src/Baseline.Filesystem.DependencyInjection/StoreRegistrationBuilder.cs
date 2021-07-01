using System;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Builder class containing properties required to fluently add an adapter registration to the
    /// <see cref="StoreManager"/> class.
    /// </summary>
    public class StoreRegistrationBuilder
    {
        /// <summary>
        /// Gets or sets the name of the store.
        /// </summary>
        internal string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the root path of the store.
        /// </summary>
        internal PathRepresentation RootPath { get; set; }
        
        /// <summary>
        /// Gets or sets the lambda function used to resolve an <see cref="IAdapter"/> instance that becomes the store's
        /// data provider.
        /// </summary>
        internal Func<IAdapter> Resolver { get; set; }
    }
}
