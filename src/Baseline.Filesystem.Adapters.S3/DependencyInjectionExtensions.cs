using System;
using Amazon.S3;
using Baseline.Filesystem.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Baseline.Filesystem.Adapters.S3
{
    public static class DependencyInjectionExtensions
    {
        public static AdapterRegistrationBuilder UsingS3Adapter(
            this AdapterRegistrationBuilder adapterRegistrationBuilder,
            Action<S3AdapterConfiguration> configurationBuilder
        )
        {
            var configuration = new S3AdapterConfiguration();
            configurationBuilder(configuration);

            return adapterRegistrationBuilder.WithAdapter(_ => new S3Adapter(configuration));
        }
        
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

    public class test
    {
        public void main()
        {
            var x = new ServiceCollection();

            x.UseBaselineFilesystem(builder =>
            {
                builder
                    .AddAdapterRegistration(registrationBuilder =>
                    {
                        registrationBuilder
                            .WithName("foo")
                            .WithRootPath("foo/bar")
                            .UsingS3Adapter((serviceProvider, config) =>
                            {
                                config.BucketName = "my-bucket";
                                config.S3Client = serviceProvider.GetService<IAmazonS3>();
                            });
                    })
                    .AddAdapterRegistration(registrationBuilder =>
                    {
                        registrationBuilder
                            .WithName("bar")
                            .UsingS3Adapter(config => config.BucketName = "foo-bar");
                    });
            });
        }
    }
}
