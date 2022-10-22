using System;

namespace Baseline.Filesystem;

/// <summary>
/// Dependency injection extensions that allow the memory adapter to be registered against a store and added to the IoC
/// container.
/// </summary>
public static class MemoryAdapterDependencyInjectionExtensions
{
    /// <summary>
    /// Configures the store registration builder to use the memory adapter.
    /// </summary>
    /// <param name="storeRegistrationBuilder">The current store registration builder instance.</param>
    /// <param name="configurationBuilder">A delegate used to configure the memory adapter.</param>
    public static StoreRegistrationBuilder UsingS3Adapter(
        this StoreRegistrationBuilder storeRegistrationBuilder,
        Action<MemoryAdapterConfiguration> configurationBuilder
    )
    {
        var configuration = new MemoryAdapterConfiguration();

        return storeRegistrationBuilder.WithAdapter(() =>
        {
            configurationBuilder(configuration);
            return new MemoryAdapter(configuration);
        });
    }
}
