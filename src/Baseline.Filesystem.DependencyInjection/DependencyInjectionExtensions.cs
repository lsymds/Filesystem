using System;
using Microsoft.Extensions.DependencyInjection;

namespace Baseline.Filesystem.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
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
                
                    foreach (var b in filesystemBuilder.AdapterRegistrations)
                    {
                        adapterManager.Register(new AdapterRegistration
                        {
                            Name = b.Name,
                            Adapter = b.Resolver(serviceProvider),
                            RootPath = b.RootPath.AsBaselineFilesystemPath()
                        });
                    }

                    return adapterManager;
                })
                .AddSingleton<IFileManager, FileManager>()
                .AddSingleton<IDirectoryManager, DirectoryManager>();
        }
    }

    public class Testing
    {
        public static void Main()
        {
            var x = new ServiceCollection();
        }
    }
}
