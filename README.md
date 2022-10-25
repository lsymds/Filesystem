# 👋 Baseline.Filesystem

A storage and filesystem abstraction layer for modern .NET projects. Save yourself the pain of writing an IFileStore over and over again and use this tried, tested, well documented and super extensible one instead. Use numerous filesystems (S3, memory, disk) via a common API.

```csharp
var storeManager = new StoreManager();
var fileManager = new FileManager(storeManager);
var directoryManager = new DirectoryManager(storeManager);

// Register a store for certificates.
storeManager.Register(new StoreRegistration
{
    Name = "certificates",
    RootPath = "student-information/certificates/".AsBaselineFilesystemPath(),
    Adapter = new S3Adapter(new S3AdapterConfiguration
    {
        // ...
    })
});

// Register a store for identification.
storeManager.Register(new StoreRegistration
{
    Name = "identification",
    RootPath = "student-information/identification/".AsBaselineFilesystemPath(),
    Adapter = new S3Adapter(new S3AdapterConfiguration
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

To learn more about contributing to the project, please read our contribution guidelines available at the documentation link below.

## 📕 Documentation

Documentation for this project is available on our project documentation site: [https://docs.getbaseline.net/filesystem/](https://docs.getbaseline.net/filesystem/).

## 🗿 Licensing

Baseline.Filesystem is licensed under the permissive MIT license. More information is available within this repository's
LICENSE file located here: [https://github.com/getbaseline/Baseline.Filesystem/blob/main/LICENSE](https://github.com/getbaseline/Baseline.Filesystem/blob/main/LICENSE)
