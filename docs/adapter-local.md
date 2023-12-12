[Go back](index.md)

# Local Adapter

LSymds.Filesystem ships with an local filesystem adapter. It implements the standard API of the project and
adheres to all of its principles around pathing, stores and so on.

There are a few things you should be aware of when using this adapter, which are detailed under the
[Things to be aware of](#things-to-be-aware-of) section.

## Getting started

This adapter is included in the standard `LSymds.Filesystem` package.

The local adapter's constructor expects a configuration object which contains the following properties:

- `GetPublicUrlToReturn` - REQUIRED - A delegate that gets a public URL to return for a given file when the `GetPublicUrlAsync` method is called.

Failure to provide a valid configuration option will result in an exception being thrown.

### Using the adapter when setting up via dependency injection

When setting up LSymds.Filesystem via dependency injection, call the `UsingLocalAdapter` on the `StoreRegistrationBuilder`
instance passed as a parameter to your configuration delegate.

```csharp
public void Configure(IServiceCollection serviceCollection)
{
    serviceCollection.UseFilesystem(filesystemBuilder =>
    {
        filesystemBuilder.AddStoreRegistration(storeRegistrationBuilder =>
        {
            storeRegistrationBuilder
                .WithName("certificates")
                .WithRootPath("/root/student-information/certificates/".AsFilesystemPath())
                .UsingLocalAdapter(c =>
                {
                    c.GetPublicUrlToReturn = file => "https://www.google.com";
                });
        });
    });
}
```

### Using the adapter when setting up manually

To use the adapter when setting up the managers manually, simply create an instance of the `LocalAdapter` class, pass in
the configuration and use that when creating a `StoreRegistration` instance.

```csharp
var storeManager = new StoreManager();

storeManager.Register(new StoreRegistration
{
    Name = "certificates",
    RootPath = "D:/student-information/certificates/".AsFilesystemPath(),
    Adapter = new LocalAdapter(new LocalAdapterConfiguration
    {
        GetPublicUrlToReturn = file => "https://www.google.com"
    })
});
```

## Things to be aware of

### Path formats

LSymds.Filesystem only supports unix-like path formats (i.e. C:/path/to/file.txt). Windows, and your applications if you
have previously persisted any paths, are likely in the format of C:\path\to\file.txt. You will need to convert the path
before LSymds.Filesystem accepts it.
