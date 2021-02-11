using System;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Extension methods for the <see cref="AdapterRegistrationBuilder"/> class.
    /// </summary>
    public static class AdapterRegistrationBuilderExtensions
    {
        /// <summary>
        /// Configures the name of the adapter.
        /// </summary>
        /// <param name="builder">The current builder instance to modify.</param>
        /// <param name="name">The name to set against the adapter.</param>
        /// <returns>The current builder instance with the name set.</returns>
        public static AdapterRegistrationBuilder WithName(this AdapterRegistrationBuilder builder, string name)
        {
            builder.Name = name;
            return builder;
        }

        /// <summary>
        /// Configures the root path of the adapter.
        /// </summary>
        /// <param name="builder">The current builder instance to modify.</param>
        /// <param name="rootPath">The root path to set against the adapter.</param>
        /// <returns>The current builder instance with the root path set.</returns>
        public static AdapterRegistrationBuilder WithRootPath(
            this AdapterRegistrationBuilder builder,
            PathRepresentation rootPath
        )
        {
            builder.RootPath = rootPath;
            return builder;
        }
        
        /// <summary>
        /// Configures the adapter implementation to use for the adapter registration.
        /// </summary>
        /// <param name="builder">The current builder instance to modify.</param>
        /// <param name="resolver">A lambda function used to resolve an <see cref="IAdapter"/> implementation.</param>
        /// <returns>The current builder instance with the adapter to use set.</returns>
        public static AdapterRegistrationBuilder WithAdapter(
            this AdapterRegistrationBuilder builder,
            Func<IServiceProvider, IAdapter> resolver
        )
        {
            builder.Resolver = resolver;
            return builder;
        }
    }
}
