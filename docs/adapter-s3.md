[Go back](index.md)

# AWS Simple Storage Service (S3) Adapter

Baseline.Filesystem ships with an AWS Simple Storage Service adapter. It implements the standard API of the project and
adheres to all of its principles around pathing, stores and so on.

There are a few things you should be aware of when using this adapter, which are detailed under the
[Things to be aware of](#things-to-be-aware-of) section.

## Getting started

This adapter is included in the standard `Baseline.Filesystem` package.

The S3 adapter's constructor expects a configuration object which contains the following properties:

- `S3Client` - REQUIRED - an instance of an S3 client, or an alternative implementation of the `IAmazonS3` interface.
- `BucketName` - REQUIRED - the name of the bucket that will be interacted with by the store.

Failure to provide a valid configuration option will result in an exception being thrown.

### Using the adapter when setting up via dependency injection

When setting up Baseline via dependency injection, call the `UsingS3Adapter` on the `StoreRegistrationBuilder` instance
passed as a parameter to your configuration delegate.

```csharp
public void Configure(IServiceCollection serviceCollection)
{
    serviceCollection.UseBaselineFilesystem(baselineFilesystemBuilder =>
    {
        baselineFilesystemBuilder.AddStoreRegistration(storeRegistrationBuilder =>
        {
            storeRegistrationBuilder
                .WithName("certificates")
                .WithRootPath("student-information/certificates/".AsBaselineFilesystemPath())
                .UsingS3Adapter(c =>
                {
                    c.S3Client = new S3Client();
                    c.BucketName = "my-bucket";
                });
        });
    });
}
```

If you wish to resolve your `IAmazonS3` instance or any other service from the dependency injection container you can
use the alternative `UseBaselineFilesystem` overload which provides you with an `IServiceProvider` instance as the first
parameter:

```csharp
public void Configure(IServiceCollection serviceCollection)
{
    serviceCollection.UseBaselineFilesystem((serviceProvider, baselineFilesystemBuilder) =>
    {
        baselineFilesystemBuilder.AddStoreRegistration(storeRegistrationBuilder =>
        {
            storeRegistrationBuilder
                .WithName("certificates")
                .WithRootPath("student-information/certificates/".AsBaselineFilesystemPath())
                .UsingS3Adapter(c =>
                {
                    var s3Options = serviceProvider.GetService<IOptions<S3Configuration>>();

                    c.S3Client = serviceProvider.GetService<IAmazonS3>();
                    c.BucketName = s3Options.Value.BucketName;
                });
        });
    });
}
```

### Using the adapter when setting up manually

To use the adapter when setting up the managers manually, simply create an instance of the `S3Adapter` class, pass in
the configuration and use that when creating a `StoreRegistration` instance.

```csharp
var storeManager = new StoreManager();

storeManager.Register(new StoreRegistration
{
    Name = "certificates",
    RootPath = "student-information/certificates/".AsBaselineFilesystemPath(),
    Adapter = new S3Adapter(new S3AdapterConfiguration
    {
        Client = new S3Client(),
        BucketName = "my-bucket"
    })
});
```

## Things to be aware of

### Request costs

For most people, you'll never make enough requests to be charged, but for a small minority making millions of file
calls, you need to understand that this adapter will often make two or more requests for every action that occurs. This
is to enforce a common API with things like checking the existence of files.

### Lack of native directory support

As S3 is a simple object store with files stored under paths, it _technically_ does not support directories. The S3
console and other applications "fake" the existence of directories by splitting the paths of all files and aggregating
them.

Unfortunately, this means that operations on directories have to enumerate and perform actions on all files under that
directory path individually. Deleting, moving or copying large directories (i.e. hundreds of thousands of files or more)
can take a long time due to this.
