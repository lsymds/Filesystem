[Go back](index.md)

# Memory Adapter

Baseline.Filesystem ships with an in memory filesystem adapter. It implements the standard API of the project and
adheres to all of its principles around pathing, stores and so on. It is very, very fast and is well suited to
test projects or projects requiring ephemeral storage.

There are a few things you should be aware of when using this adapter, which are detailed under the
[Things to be aware of](#things-to-be-aware-of) section.

## Getting started

This adapter is included in the standard `Baseline.Filesystem` package.

The memory adapter's constructor expects a configuration object which contains the following properties:

- `PublicUrlToReturn` - REQUIRED - A URL to return when the `GetPublicUrlAsync` method is called on the FileManager.

Failure to provide a valid configuration option will result in an exception being thrown.

### Using the adapter when setting up via dependency injection

When setting up Baseline via dependency injection, call the `UsingMemoryAdapter` on the `StoreRegistrationBuilder`
instance passed as a parameter to your configuration delegate.

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
                .UsingMemoryAdapter(c =>
                {
                    c.PublicUrlToReturn = "https://www.google.com";
                });
        });
    });
}
```

### Using the adapter when setting up manually

To use the adapter when setting up the managers manually, simply create an instance of the `MemoryAdapter` class, pass in
the configuration and use that when creating a `StoreRegistration` instance.

```csharp
var storeManager = new StoreManager();

storeManager.Register(new StoreRegistration
{
    Name = "certificates",
    RootPath = "student-information/certificates/".AsBaselineFilesystemPath(),
    Adapter = new MemoryAdapter(new MemoryAdapterConfiguration
    {
        PublicUrlToReturn = "https://www.google.com"
    })
});
```

## Things to be aware of

### Memory limitations

As the filesystem is stored in memory, you're limited to the amount of memory you have on the machine your application
is running on. If you'll be working with extraordinarily large files, you'd be better off picking an alternative
adapter.
