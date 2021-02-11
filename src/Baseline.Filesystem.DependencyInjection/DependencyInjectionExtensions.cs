using System;
using Microsoft.Extensions.DependencyInjection;

namespace Baseline.Filesystem
{
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
        /// A lambda function used to fluently configure the Baseline.Filesystem dependencies, adding adapters and
        /// default configurations.
        /// </param>
        /// <returns>
        /// The same service collection now configures with the Baseline.Filesystem dependencies to enable fluent
        /// service registration.
        /// </returns>
        public static IServiceCollection UseBaselineFilesystem(
            this IServiceCollection serviceCollection,
            Action<BaselineFilesystemBuilder> builder
        )
        {
            var filesystemBuilder = new BaselineFilesystemBuilder();
            builder(filesystemBuilder);
            
            return serviceCollection
                .AddSingleton<IAdapterManager>(serviceProvider =>
                {
                    var adapterManager = new AdapterManager();
                
                    foreach (var adapterRegistration in filesystemBuilder.AdapterRegistrations)
                    {
                        adapterManager.Register(new AdapterRegistration
                        {
                            Name = adapterRegistration.Name,
                            Adapter = adapterRegistration.Resolver(serviceProvider),
                            RootPath = adapterRegistration.RootPath
                        });
                    }

                    return adapterManager;
                })
                .AddSingleton<IFileManager, FileManager>()
                .AddSingleton<IDirectoryManager, DirectoryManager>();
        }
    }
}
