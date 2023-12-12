[Go back](index.md)

# Working with Paths

All paths within the library are represented by a `PathRepresentation` instance. This object provides a helpful wrapper
around what would otherwise be a string and gives a number of utility methods that can be used to glean more information
about the path itself.

## Creating paths

You can create a path in two ways: by writing a path as a string and using an extension method to convert it or by
instantiating an instance of the `PathRepresentation` object from scratch.

### Creating a path from a string

To create a path from a string call the `AsFilesystemPath` extension method. For example:

```csharp
var path = "path/to/my/file.txt".AsFilesystemPath();
```

### Instantiating a path instance directly

To instantiate an instance of the `PathRepresentation` object directly, call its constructor and pass your path in as
its first argument. For example:

```csharp
var path = new PathRepresentation("path/to/my/file.txt");
```
