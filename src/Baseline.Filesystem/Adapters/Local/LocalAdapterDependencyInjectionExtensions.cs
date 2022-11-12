using System;

namespace Baseline.Filesystem;

/// <summary>
/// Dependency injection extensions that allow the local adapter to be registered against a store and added to the IoC
/// container.
/// </summary>
public static class LocalAdapterDependencyInjectionExtensions
{
    /// <summary>
    /// Configures the store registration builder to use the local adapter.
    /// </summary>
    /// <param name="storeRegistrationBuilder">The current store registration builder instance.</param>
    /// <param name="configurationBuilder">A delegate used to configure the local adapter.</param>
    public static StoreRegistrationBuilder UsingLocalAdapter(
        this StoreRegistrationBuilder storeRegistrationBuilder,
        Action<LocalAdapterConfiguration> configurationBuilder
    )
    {
        var configuration = new LocalAdapterConfiguration();

        return storeRegistrationBuilder.WithAdapter(() =>
        {
            configurationBuilder(configuration);
            return new LocalAdapter(configuration);
        });
    }
}
