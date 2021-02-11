using System;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Dependency injection extensions that allow the S3 adapter to be added to the IoC container.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Configures the adaption registration builder to use the S3 adapter.
        /// </summary>
        /// <param name="adapterRegistrationBuilder">The current adapter registration builder instance.</param>
        /// <param name="configurationBuilder">A lambda function used to configure the S3 configuration file.</param>
        /// <returns>The current adapter registration builder configured with the S3 adapter.</returns>
        public static AdapterRegistrationBuilder UsingS3Adapter(
            this AdapterRegistrationBuilder adapterRegistrationBuilder,
            Action<S3AdapterConfiguration> configurationBuilder
        )
        {
            var configuration = new S3AdapterConfiguration();
            configurationBuilder(configuration);

            return adapterRegistrationBuilder.WithAdapter(_ => new S3Adapter(configuration));
        }
        
        /// <summary>
        /// Configures the adaption registration builder to use the S3 adapter.
        /// </summary>
        /// <param name="adapterRegistrationBuilder">The current adapter registration builder instance.</param>
        /// <param name="configurationBuilder">A lambda function used to configure the S3 configuration file.</param>
        /// <returns>The current adapter registration builder configured with the S3 adapter.</returns>
        public static AdapterRegistrationBuilder UsingS3Adapter(
            this AdapterRegistrationBuilder adapterRegistrationBuilder,
            Action<IServiceProvider, S3AdapterConfiguration> configurationBuilder
        )
        {
            var configuration = new S3AdapterConfiguration();

            return adapterRegistrationBuilder.WithAdapter(s =>
            {
                configurationBuilder(s, configuration);
                return new S3Adapter(configuration);
            });
        }
    }
}
