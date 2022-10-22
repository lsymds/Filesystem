using System;

namespace Baseline.Filesystem;

/// <summary>
/// Dependency injection extensions that allow the S3 adapter to be registered against a store and added to the IoC
/// container.
/// </summary>
public static class S3AdapterDependencyInjectionExtensions
{
    /// <summary>
    /// Configures the store registration builder to use the S3 adapter.
    /// </summary>
    /// <param name="storeRegistrationBuilder">The current store registration builder instance.</param>
    /// <param name="configurationBuilder">A lambda function used to configure the S3 configuration file.</param>
    public static StoreRegistrationBuilder UsingS3Adapter(
        this StoreRegistrationBuilder storeRegistrationBuilder,
        Action<S3AdapterConfiguration> configurationBuilder
    )
    {
        var configuration = new S3AdapterConfiguration();

        return storeRegistrationBuilder.WithAdapter(() =>
        {
            configurationBuilder(configuration);
            return new S3Adapter(configuration);
        });
    }
}
