using System;

namespace LSymds.Filesystem;

/// <summary>
/// Extension methods for the <see cref="FilesystemBuilder" /> class.
/// </summary>
public static class FilesystemBuilderExtensions
{
    /// <summary>
    /// Adds a store registration to be included in the built <see cref="IStoreManager"/> class. The name of
    /// the store defaults to "default", and there is no requirement for a root path to be included.
    /// </summary>
    /// <param name="builder">The current builder instance.</param>
    /// <param name="storeRegistrationBuilder">
    /// A lambda function used to modify a new store registration builder.
    /// </param>
    public static FilesystemBuilder AddStoreRegistration(
        this FilesystemBuilder builder,
        Action<StoreRegistrationBuilder> storeRegistrationBuilder
    )
    {
        var registration = new StoreRegistrationBuilder();
        storeRegistrationBuilder(registration);

        builder.StoreRegistrations.Add(registration);

        return builder;
    }
}
