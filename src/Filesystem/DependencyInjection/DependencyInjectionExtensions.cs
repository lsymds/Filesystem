using System;
using Microsoft.Extensions.DependencyInjection;

namespace LSymds.Filesystem;

/// <summary>
/// Extension methods that add the Baseline.Filesystem project to an <see cref="IServiceCollection"/> implementation.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds the Baseline.Filesystem dependencies to the service collection container in a fluent way. The
    /// interface implementations (IAdapterManager, IFileManager, IDirectoryManager) are all registered as
    /// singletons.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add the Baseline.Filesystem dependencies to.</param>
    /// <param name="builder">
    /// A lambda function used to fluently configure the Baseline.Filesystem dependencies, adding stores and
    /// default configurations.
    /// </param>
    public static IServiceCollection UseBaselineFilesystem(
        this IServiceCollection serviceCollection,
        Action<BaselineFilesystemBuilder> builder
    )
    {
        return serviceCollection.UseBaselineFilesystem(
            (_, filesystemBuilder) => builder(filesystemBuilder)
        );
    }

    /// <summary>
    /// Adds the Baseline.Filesystem dependencies to the service collection container in a fluent way. The
    /// interface implementations (IAdapterManager, IFileManager, IDirectoryManager) are all registered as
    /// singletons.
    /// </summary>
    /// <param name="serviceCollection">The service collection to add the Baseline.Filesystem dependencies to.</param>
    /// <param name="builder">
    /// A lambda function used to fluently configure the Baseline.Filesystem dependencies, adding stores and
    /// default configurations.
    /// </param>
    public static IServiceCollection UseBaselineFilesystem(
        this IServiceCollection serviceCollection,
        Action<IServiceProvider, BaselineFilesystemBuilder> builder
    )
    {
        return serviceCollection
            .AddSingleton<IStoreManager>(serviceProvider =>
            {
                var filesystemBuilder = new BaselineFilesystemBuilder();
                builder(serviceProvider, filesystemBuilder);

                var storeManager = new StoreManager();

                foreach (var storeRegistration in filesystemBuilder.StoreRegistrations)
                {
                    storeManager.Register(
                        new StoreRegistration
                        {
                            Name = storeRegistration.Name,
                            Adapter = storeRegistration.Resolver(),
                            RootPath = storeRegistration.RootPath
                        }
                    );
                }

                return storeManager;
            })
            .AddSingleton<IFileManager, FileManager>()
            .AddSingleton<IDirectoryManager, DirectoryManager>();
    }
}
