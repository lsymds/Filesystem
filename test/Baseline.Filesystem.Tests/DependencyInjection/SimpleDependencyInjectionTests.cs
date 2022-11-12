using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Baseline.Filesystem.Tests.DependencyInjection;

public class SimpleDependencyInjectionTests
{
    [Fact]
    public void It_Can_Register_A_Single_Store()
    {
        // Act.
        var serviceCollection = new ServiceCollection();
        serviceCollection.UseBaselineFilesystem(baselineFilesystemBuilder =>
        {
            baselineFilesystemBuilder.AddStoreRegistration(storeRegistrationBuilder =>
            {
                storeRegistrationBuilder
                    .WithName("default")
                    .UsingS3Adapter(adapterConfiguration =>
                    {
                        adapterConfiguration.BucketName = "Blah";
                        adapterConfiguration.S3Client = new AmazonS3Client(
                            new BasicAWSCredentials("abc", "def"),
                            new AmazonS3Config
                            {
                                AuthenticationRegion = "eu-west-1",
                                ServiceURL = "http://localhost:4566",
                                ForcePathStyle = true,
                            }
                        );
                    });
            });
        });
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Assert.
        var storeManager = serviceProvider.GetService<IStoreManager>();
        storeManager!.Get("default").Adapter.Should().BeOfType<S3Adapter>();
    }

    [Fact]
    public void It_Can_Register_A_Store_Using_The_Service_Provider()
    {
        // Act.
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(
            new AmazonS3Client(
                new BasicAWSCredentials("abc", "def"),
                new AmazonS3Config
                {
                    AuthenticationRegion = "eu-west-1",
                    ServiceURL = "http://localhost:4566",
                    ForcePathStyle = true,
                }
            )
        );
        serviceCollection.UseBaselineFilesystem(
            (services, baselineFilesystemBuilder) =>
            {
                baselineFilesystemBuilder.AddStoreRegistration(storeRegistrationBuilder =>
                {
                    storeRegistrationBuilder
                        .WithName("default")
                        .UsingS3Adapter(adapterConfiguration =>
                        {
                            adapterConfiguration.BucketName = "qqwdqwd";
                            adapterConfiguration.S3Client = services.GetService<IAmazonS3>();
                        });
                });
            }
        );
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Assert.
        var storeManager = serviceProvider.GetService<IStoreManager>();
        storeManager!.Get("default").Adapter.Should().BeOfType<S3Adapter>();
    }

    [Fact]
    public void It_Can_Register_Multiple_Stores()
    {
        // Arrange.
        var client = new AmazonS3Client(
            new BasicAWSCredentials("abc", "def"),
            new AmazonS3Config
            {
                AuthenticationRegion = "eu-west-1",
                ServiceURL = "http://localhost:4566",
                ForcePathStyle = true,
            }
        );

        // Act.
        var serviceCollection = new ServiceCollection();
        serviceCollection.UseBaselineFilesystem(baselineFilesystemBuilder =>
        {
            baselineFilesystemBuilder.AddStoreRegistration(storeRegistrationBuilder =>
            {
                storeRegistrationBuilder
                    .WithName("default")
                    .UsingS3Adapter(adapterConfiguration =>
                    {
                        adapterConfiguration.BucketName = "dqwdqwd";
                        adapterConfiguration.S3Client = client;
                    });
            });

            baselineFilesystemBuilder.AddStoreRegistration(storeRegistrationBuilder =>
            {
                storeRegistrationBuilder
                    .WithName("second")
                    .WithRootPath("abc/".AsBaselineFilesystemPath())
                    .UsingMemoryAdapter(c =>
                    {
                        c.PublicUrlToReturn = "https://www.google.com";
                    });
            });

            baselineFilesystemBuilder.AddStoreRegistration(storeRegistrationBuilder =>
            {
                storeRegistrationBuilder
                    .WithName("third")
                    .WithRootPath("abc/".AsBaselineFilesystemPath())
                    .UsingLocalAdapter(c =>
                    {
                        c.GetPublicUrlForPath = _ => "https://www.google.com";
                    });
            });
        });
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Assert.
        var storeManager = serviceProvider.GetService<IStoreManager>();
        storeManager!.Get("default").Adapter.Should().BeOfType<S3Adapter>();
        storeManager!.Get("second").Adapter.Should().BeOfType<MemoryAdapter>();
    }
}
