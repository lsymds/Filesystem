# 👋 Filesystem

Filesystem abstractions for modern .NET projects. Interact with numerous filesystems (S3, memory, disk) at once via a
single abstraction.

```csharp
var storeManager = new StoreManager();
var fileManager = new FileManager(storeManager);
var directoryManager = new DirectoryManager(storeManager);

// Register a store for certificates.
storeManager.Register(new StoreRegistration
{
    Name = "certificates",
    RootPath = "student-information/certificates/".AsFilesystemPath(),
    Adapter = new S3Adapter(new S3AdapterConfiguration
    {
        // ...
    })
});

// Store temporary log files in memory.
storeManager.Register(new StoreRegistration
{
    Name = "temporary_logs",
    Adapter = new MemoryAdapter(new MemoryAdapterConfiguration
    {
        // ...
    })
});

// Store SSH keys on disk.
storeManager.register(new StoreRegistration
{
    Name = "ssh_keys",
    Adapter = new LocalAdapter(new LocalAdapterConfiguration
    {
        // ...
    })
});

// Create an empty file at b-smith/graduation_certificate within the certificates adapter (true
// path will be student-information/certificates/b-smith/graduation_certificate).
await fileManager.TouchFileAsync(
    new TouchFileRequest { Path = "b-smith/graduation_certificate" },
    "certificates"
);
```

## 👥 Contributing

We accept contributions in the form of feature requests, bug reports and suggestions. We do not accept code contributions.

## 📕 Documentation

Documentation for this project is available in the [docs/ folder within this repository](docs/index.md).

## 🗿 Licensing

LSymds.Filesystem is licensed under the permissive MIT license. More information is available within this repository's
LICENSE file located [HERE](LICENSE).
