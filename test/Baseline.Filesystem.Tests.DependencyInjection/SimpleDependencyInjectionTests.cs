using System;
using System.Threading.Tasks;
using Amazon.S3;
using Baseline.Filesystem.Tests.Adapters.S3.Integration;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Baseline.Filesystem.Tests.DependencyInjection
{
    public class SimpleDependencyInjectionTests : BaseS3AdapterIntegrationTest
    {
        [Fact]
        public async Task It_Can_Register_A_Single_Adapter()
        {
            // Arrange.
            var filePath = RandomFilePathRepresentation();
            await CreateFileAndWriteTextAsync(filePath, "hello, world");
            
            // Act.
            var serviceCollection = new ServiceCollection();
            serviceCollection.UseBaselineFilesystem(baselineFilesystemBuilder =>
            {
                baselineFilesystemBuilder.AddAdapterRegistration(adapterRegistrationBuilder =>
                {
                    adapterRegistrationBuilder
                        .WithName("default")
                        .UsingS3Adapter(adapterConfiguration =>
                        {
                            adapterConfiguration.BucketName = GeneratedBucketName;
                            adapterConfiguration.S3Client = S3Client;
                        });
                });
            });
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert.
            var adapterManager = serviceProvider.GetService<IAdapterManager>();
            adapterManager!.Get("default").Adapter.Should().BeOfType<S3Adapter>();

            var fileManager = serviceProvider.GetService<IFileManager>();
            var fileContents = await fileManager!.ReadAsStringAsync(
                new ReadFileAsStringRequest
                {
                    FilePath = filePath
                }
            );
            fileContents.Should().Be("hello, world");
        }

        [Fact]
        public async Task It_Can_Register_An_Adapter_Using_The_Service_Provider()
        {
            // Arrange.
            var filePath = RandomFilePathRepresentation();
            await CreateFileAndWriteTextAsync(filePath, "hello, world");
            
            // Act.
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IAmazonS3>(S3Client);
            serviceCollection.UseBaselineFilesystem(baselineFilesystemBuilder =>
            {
                baselineFilesystemBuilder.AddAdapterRegistration(adapterRegistrationBuilder =>
                {
                    adapterRegistrationBuilder
                        .WithName("default")
                        .UsingS3Adapter((services, adapterConfiguration) =>
                        {
                            adapterConfiguration.BucketName = GeneratedBucketName;
                            adapterConfiguration.S3Client = services.GetService<IAmazonS3>();
                        });
                });
            });
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert.
            var adapterManager = serviceProvider.GetService<IAdapterManager>();
            adapterManager!.Get("default").Adapter.Should().BeOfType<S3Adapter>();

            var fileManager = serviceProvider.GetService<IFileManager>();
            var fileContents = await fileManager!.ReadAsStringAsync(
                new ReadFileAsStringRequest
                {
                    FilePath = filePath
                }
            );
            fileContents.Should().Be("hello, world");
        }
        
        [Fact]
        public async Task It_Can_Register_Multiple_Adapters()
        {
            // Arrange.
            var filePath = RandomFilePathRepresentation();
            await CreateFileAndWriteTextAsync(filePath, "hello, world");
            
            // Act.
            var serviceCollection = new ServiceCollection();
            serviceCollection.UseBaselineFilesystem(baselineFilesystemBuilder =>
            {
                baselineFilesystemBuilder.AddAdapterRegistration(adapterRegistrationBuilder =>
                {
                    adapterRegistrationBuilder
                        .WithName("default")
                        .UsingS3Adapter(adapterConfiguration =>
                        {
                            adapterConfiguration.BucketName = GeneratedBucketName;
                            adapterConfiguration.S3Client = S3Client;
                        });
                });

                baselineFilesystemBuilder.AddAdapterRegistration(adapterRegistrationBuilder =>
                {
                    adapterRegistrationBuilder
                        .WithName("second")
                        .UsingS3Adapter(adapterConfiguration =>
                        {
                            adapterConfiguration.BucketName = GeneratedBucketName;
                            adapterConfiguration.S3Client = S3Client;
                        });
                });
            });
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert.
            var adapterManager = serviceProvider.GetService<IAdapterManager>();
            adapterManager!.Get("default").Adapter.Should().BeOfType<S3Adapter>();
            adapterManager!.Get("second").Adapter.Should().BeOfType<S3Adapter>();

            var fileManager = serviceProvider.GetService<IFileManager>();
            var fileContents = await fileManager!.ReadAsStringAsync(
                new ReadFileAsStringRequest
                {
                    FilePath = filePath
                },
                "second"
            );
            fileContents.Should().Be("hello, world");
        }
        [Fact]
        public async Task It_Functions_Correctly_When_A_Root_Path_Is_Registered()
        {
            // Arrange.
            ReconfigureManagerInstances(true);
            
            var filePath = RandomFilePathRepresentation();
            await CreateFileAndWriteTextAsync(filePath, "hello, world");
            
            // Act.
            var serviceCollection = new ServiceCollection();
            serviceCollection.UseBaselineFilesystem(baselineFilesystemBuilder =>
            {
                baselineFilesystemBuilder.AddAdapterRegistration(adapterRegistrationBuilder =>
                {
                    adapterRegistrationBuilder
                        .WithName("default")
                        .WithRootPath(RootPath.AsBaselineFilesystemPath())
                        .UsingS3Adapter(adapterConfiguration =>
                        {
                            adapterConfiguration.BucketName = GeneratedBucketName;
                            adapterConfiguration.S3Client = S3Client;
                        });
                });
            });
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Assert.
            var adapterManager = serviceProvider.GetService<IAdapterManager>();
            adapterManager!.Get("default").Adapter.Should().BeOfType<S3Adapter>();

            var fileManager = serviceProvider.GetService<IFileManager>();
            var fileContents = await fileManager!.ReadAsStringAsync(
                new ReadFileAsStringRequest
                {
                    FilePath = filePath
                }
            );
            fileContents.Should().Be("hello, world");
        }
    }
}
