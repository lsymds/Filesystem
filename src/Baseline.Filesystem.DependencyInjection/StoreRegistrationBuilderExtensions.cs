using System;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Extension methods for the <see cref="StoreRegistrationBuilder"/> class.
    /// </summary>
    public static class StoreRegistrationBuilderExtensions
    {
        /// <summary>
        /// Configures the name of the store.
        /// </summary>
        /// <param name="builder">The current builder instance to modify.</param>
        /// <param name="name">The name to set against the store.</param>
        public static StoreRegistrationBuilder WithName(this StoreRegistrationBuilder builder, string name)
        {
            builder.Name = name;
            return builder;
        }

        /// <summary>
        /// Configures the root path of the store.
        /// </summary>
        /// <param name="builder">The current builder instance to modify.</param>
        /// <param name="rootPath">The root path to set against the store.</param>
        public static StoreRegistrationBuilder WithRootPath(
            this StoreRegistrationBuilder builder,
            PathRepresentation rootPath
        )
        {
            builder.RootPath = rootPath;
            return builder;
        }
        
        /// <summary>
        /// Configures the adapter implementation to use for the store.
        /// </summary>
        /// <param name="builder">The current builder instance to modify.</param>
        /// <param name="resolver">A lambda function used to resolve an <see cref="IAdapter"/> implementation.</param>
        public static StoreRegistrationBuilder WithAdapter(
            this StoreRegistrationBuilder builder,
            Func<IAdapter> resolver
        )
        {
            builder.Resolver = resolver;
            return builder;
        }
    }
}
