[Go back](index.md)

# Basic Usage

## Installation

Installation of the project is quick and easy thanks to the NuGet package manager. Search for `Baseline.Filesystem` and
install the `Baseline.Filesystem` package. This package includes all of the default adapters as well as support for
dependency injection via the `Microsoft.Extensions.DependencyInjection` package.

## Configuring

Configuring the library is quick and easy depending on whether you use dependency injection or not. The same rules
apply regardless: you register a store with an optional name and root path that has an adapter with many configuration
options backing it.

### Configuring via dependency injection

> [!TIP]
> All services within Baseline.Filesystem are registered as singletons.

Given an `IServiceCollection` that can be modified (in your `Startup.cs` file for example), call the
`UseBaselineFilesystem` method on the service collection.

This method accepts a delegate with either a single parameter which is a builder object or two parameters where the
first is an `IServiceProvider` instance used to resolve additional service and the second is a
builder object.

```csharp
public void Configure(IServiceCollection serviceCollection)
{
    serviceCollection.UseBaselineFilesystem(baselineFilesystemBuilder =>
    {
        // ...
    });

    // or...

    serviceCollection.UseBaselineFilesystem((serviceProvider, baselineFilesystemBuilder) =>
    {
        // ...
    });
}
```

The filesystem builder object has a single extension method on it that allows you to add a store registration via the
`AddStoreRegistration` method. This method again accepts a single parameter which is a delegate which configures the
object.

```csharp
public void Configure(IServiceCollection serviceCollection)
{
    serviceCollection.UseBaselineFilesystem(baselineFilesystemBuilder =>
    {
        baselineFilesystemBuilder.AddStoreRegistration(storeRegistrationBuilder =>
        {
            // ...
        });
    });
}
```

> [!TIP]
> To add more than one store simply call the `AddStoreRegistration` method multiple times. Make sure you provide unique store names!

The store registration builder then has a number of helpful extension methods on which can be used to configure the
store. Backing adapters (i.e. S3) provide their own extension methods and have their own documentation but follow the
naming convention of `UsingXAdapter` with one or more configuration options/delegates.

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
                    //...
                });
        });
    });
}
```

> [!TIP]
> Not specifying the store's naming or setting it to default will mean you do not have to specify it as a parameter when using the available file manager/directory manager methods.

### Configuring manually

If, for whatever reason, you choose not to use dependency injection, you can instantiate all of the required dependencies
manually.

To start, create an instance of the `StoreManager` class.

```csharp
var storeManager = new StoreManager();
```

Then, add your required stores, manually configuring their adapters:

```csharp
var storeManager = new StoreManager();

storeManager.Register(new StoreRegistration
{
    Name = "certificates",
    RootPath = "student-information/certificates/".AsBaselineFilesystemPath(),
    Adapter = new S3Adapter(new S3AdapterConfiguration
    {
        // ...
    })
});
```

> [!TIP]
> Not specifying the store's naming or setting it to default will mean you do not have to specify it as a parameter when using the available file manager/directory manager methods.

Finally, instantiate an instance of the `FileManager` and `DirectoryManager` classes, passing your configured
`StoreManager` as a dependency to their constructors.

```csharp
var storeManager = new StoreManager();

storeManager.Register(new StoreRegistration
{
    Name = "certificates",
    RootPath = "student-information/certificates/".AsBaselineFilesystemPath(),
    Adapter = new S3Adapter(new S3AdapterConfiguration
    {
        // ...
    })
});

var fileManager = new FileManager(storeManager);
var directoryManager = new DirectoryManager(directoryManager);
```

You can then use the `IFileManager` and `IDirectoryManager` implementations by using the instances you just created.

## Getting started

If you're using dependency injection, inject the `IFileManager` and/or `IDirectoryManager` (or their concrete
implementations `FileManager` and `DirectoryManager`) into your class' constructor.

Then, call any of the available methods. They usually follow a standard signature as follows:

```csharp
Task<DoSomethingResponse> DoSomethingAsync(
    DoSomethingRequest request,
    string store = "default",
    CancellationToken cancellationToken = default
);
```

> [!TIP]
> If you're using default as a store name, you do not need to specify it (but you can, if you really **really** want to).

## Exception handling

By default, Baseline.Filesystem will wrap any non-Baseline-managed exception to make it easier to distinguish in your
codebase what exceptions have been thrown by the library and what exceptions have been thrown by your application code
or other dependencies. The wrapping exception is named `StoreAdapterOperationException`. If you wish to access the
true exception that was thrown then that is assigned to the `InnerException` property.

If you wish to catch any Baseline.Filesystem related exceptions then you can catch the base class:
`BaselineFilesystemException`.
