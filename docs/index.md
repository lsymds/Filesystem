# Overview

Filesystem abstractions for modern .NET projects. Interact with numerous filesystems (S3, memory, disk) at once via a single abstraction.

- [Overview](index.md)
- [Basic Usage](basic-usage.md)
- [Working With Paths](working-with-paths.md)
- [Interacting With A Store's Filesystem](interacting-with-a-stores-filesystem.md)
- Storage Adapters
  - [AWS Simple Storage Service](adapter-s3.md)
  - [Memory](adapter-memory.md)
  - [Local Disk](adapter-local.md)

## Architecture

The project and all of its dependencies are contained within one easy to use package named `LSymds.Filesystem`.
Previous versions contained numerous assemblies with all of the dependencies split but this a) increased management
b) increased cognitive overload and c) increased the barrier to entry of using the package.

All public facing code is available under the `LSymds.Filesystem` namespace.

### Stores

When using the library you will see that you have the ability to define stores. Stores are instances of a provider's
adapter that point to a location within that provider's data store. You can register multiple stores with the library
and access them by using their name. Stores do not have to use different providers, but they can. This opens up a
hugely flexible filesystem interface that you can use throughout your application using one core dependency.

### Adapters

Adapters provide implementations of the core adapter interface for providers such as S3, Azure Blob Store, local
storage etc. They manipulate the providers SDK to adhere to the libraries contracts ensuring your code behaves the
same no matter what provider you are using.

### Root paths

Each store registration can be configured with a root path. This allows you to move the location data is stored around
without it impacting your application's code.

For example, if I had a 'certificates' store that I used to manage storage of student certificates, I could use the
`students/information/certificates/` path as the root path and interact with the filesystem without knowing that
path exists. This leads to hugely flexible code that doesn't rely on a filesystem that your application has no knowledge
of and instead manages files and directories that it knows how to manage.

LSymds.Filesystem will automatically handle stripping out this root path before it reaches your application, ensuring
your storage implementation details are never leaked where they shouldn't be.
