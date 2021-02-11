using System;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Builder class containing properties required to fluently add an adapter registration to the
    /// <see cref="AdapterManager"/> class.
    /// </summary>
    public class AdapterRegistrationBuilder
    {
        /// <summary>
        /// Gets or sets the name of the adapter.
        /// </summary>
        internal string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the root path of the adapter.
        /// </summary>
        internal PathRepresentation RootPath { get; set; }
        
        /// <summary>
        /// Gets or sets the lambda function used to resolve an <see cref="IAdapter"/> instance that becomes the adapter
        /// registration.
        /// </summary>
        internal Func<IServiceProvider, IAdapter> Resolver { get; set; }
    }
}
