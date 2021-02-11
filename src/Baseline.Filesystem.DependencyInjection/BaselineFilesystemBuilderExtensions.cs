using System;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Extension methods for the <see cref="BaselineFilesystemBuilder" /> class.
    /// </summary>
    public static class BaselineFilesystemBuilderExtensions
    {
        /// <summary>
        /// Adds an adapter registration to be included in the built <see cref="IAdapterManager"/> class. The name of
        /// the adapter defaults to "default", and there is no requirement for a root path to be included.
        /// </summary>
        /// <param name="builder">The current builder instance.</param>
        /// <param name="adapterRegistrationBuilder">
        /// A lambda function used to modify a new adapter registration builder.
        /// </param>
        /// <returns>The current builder instance, with the configured adapter registration added to it.</returns>
        public static BaselineFilesystemBuilder AddAdapterRegistration(
            this BaselineFilesystemBuilder builder,
            Action<AdapterRegistrationBuilder> adapterRegistrationBuilder)
        {
            var registration = new AdapterRegistrationBuilder();
            adapterRegistrationBuilder(registration);
            
            builder.AdapterRegistrations.Add(registration);

            return builder;
        }
    }
}
