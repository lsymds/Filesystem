using System.Threading.Tasks;
using Amazon.S3;
using Baseline.Filesystem.Tests.Adapters.S3;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Baseline.Filesystem.Tests.DependencyInjection;

public class SimpleDependencyInjectionTests : BaseS3AdapterIntegrationTest
{
    [Fact]
    public async Task It_Can_Register_A_Single_Store()
    {
        // Arrange.
        var filePath = RandomFilePathRepresentation();
        await CreateFileAndWriteTextAsync(filePath, "hello, world");

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
                        adapterConfiguration.BucketName = GeneratedBucketName;
                        adapterConfiguration.S3Client = S3Client;
                    });
            });
        });
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Assert.
        var storeManager = serviceProvider.GetService<IStoreManager>();
        storeManager.Get("default").Adapter.Should().BeOfType<S3Adapter>();

        var fileManager = serviceProvider.GetService<IFileManager>();
        var fileContents = await fileManager.ReadAsStringAsync(
            new ReadFileAsStringRequest { FilePath = filePath }
        );
        fileContents.FileContents.Should().Be("hello, world");
    }

    [Fact]
    public async Task It_Can_Register_A_Store_Using_The_Service_Provider()
    {
        // Arrange.
        var filePath = RandomFilePathRepresentation();
        await CreateFileAndWriteTextAsync(filePath, "hello, world");

        // Act.
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(S3Client);
        serviceCollection.UseBaselineFilesystem(
            (services, baselineFilesystemBuilder) =>
            {
                baselineFilesystemBuilder.AddStoreRegistration(storeRegistrationBuilder =>
                {
                    storeRegistrationBuilder
                        .WithName("default")
                        .UsingS3Adapter(adapterConfiguration =>
                        {
                            adapterConfiguration.BucketName = GeneratedBucketName;
                            adapterConfiguration.S3Client = services.GetService<IAmazonS3>();
                        });
                });
            }
        );
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Assert.
        var storeManager = serviceProvider.GetService<IStoreManager>();
        storeManager.Get("default").Adapter.Should().BeOfType<S3Adapter>();

        var fileManager = serviceProvider.GetService<IFileManager>();
        var fileContents = await fileManager.ReadAsStringAsync(
            new ReadFileAsStringRequest { FilePath = filePath }
        );
        fileContents.FileContents.Should().Be("hello, world");
    }

    [Fact]
    public async Task It_Can_Register_Multiple_Stores()
    {
        // Arrange.
        var filePath = RandomFilePathRepresentation();
        await CreateFileAndWriteTextAsync(filePath, "hello, world");

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
                        adapterConfiguration.BucketName = GeneratedBucketName;
                        adapterConfiguration.S3Client = S3Client;
                    });
            });

            baselineFilesystemBuilder.AddStoreRegistration(storeRegistrationBuilder =>
            {
                storeRegistrationBuilder
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
        var storeManager = serviceProvider.GetService<IStoreManager>();
        storeManager.Get("default").Adapter.Should().BeOfType<S3Adapter>();
        storeManager.Get("second").Adapter.Should().BeOfType<S3Adapter>();

        var fileManager = serviceProvider.GetService<IFileManager>();
        var fileContents = await fileManager.ReadAsStringAsync(
            new ReadFileAsStringRequest { FilePath = filePath },
            "second"
        );
        fileContents.FileContents.Should().Be("hello, world");
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
            baselineFilesystemBuilder.AddStoreRegistration(storeRegistrationBuilder =>
            {
                storeRegistrationBuilder
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
        var storeManager = serviceProvider.GetService<IStoreManager>();
        storeManager.Get("default").Adapter.Should().BeOfType<S3Adapter>();

        var fileManager = serviceProvider.GetService<IFileManager>();
        var fileContents = await fileManager.ReadAsStringAsync(
            new ReadFileAsStringRequest { FilePath = filePath }
        );
        fileContents.FileContents.Should().Be("hello, world");
    }
}
