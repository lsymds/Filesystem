[Go back](index.md)

# Interacting with a Store's Filesystem

You already know that you can interact with individual stores and that individual stores utilise an adapter to provide
the platform's filesystem specific implementations of the generic contracts supplied by the library.

What you don't know, however, is what methods are available to you to use to manipulate files and directories within
those filesystems. Read on to find out more.

## Directories

> [!TIP]
> When interacting with directory paths make sure you explicitly define the path as a directory with a terminating slash (i.e. path/to/my/directory/). An exception will be thrown if you don't.

Directories are managed by the `IDirectoryManager` interface or its concrete implementation `DirectoryManager`. Numerous
methods are available, some of which relate to multiple files and/or directories contained _within_ a directory.

It is important to understand that some adapters do not natively support directories (i.e. S3). Thus, when performing
mutating actions on directories such as copying, moving or deleting, you're not mutating the directory in one request
but having to mutate all files contained therein. Think carefully before performing a large mutating action on a directory
without first consulting the adapter's directory.

### Copying a directory

You can copy a directory by calling the `DirectoryManager.CopyAsync` method. It takes a request containing the
source directory to copy and the destination directory to copy it to and returns a response containing the destination
directory path.

An exception will be thrown if the source directory does not exist or the destination directory already exists.

### Creating a directory

You can create a directory by calling the `DirectoryManager.CreateAsync` method and passing the directory to create
in the request.

Where the store's adapter does not support directories natively (i.e. S3), a fake directory utilising a `.keep` file
will be used instead.

An exception will be thrown if the directory already exists.

### Deleting a directory

You can delete a directory by calling the `DirectoryManager.DeleteAsync` method and passing the directory to delete in
the request.

An exception will be thrown if the directory does not exist.

### Iterating over contents within a directory

You can programmatically iterate over all files and sub-directories within a directory by using the
`DirectoryManager.IterateContentsAsync` method. The request for this method accepts a directory to iterate and a delegate
that performs an action on each path as it is retrieved.

Sub-directories will be returned prior to any files under them.

An exception will be thrown if the directory to iterate over does not exist.

An example of a use of the iteration method would be to cache the list of files within a store but without retrieving
all of the contents first.

You can stop any further iterations occurring by returning false from the delegate. If you would like iterations
to continue, you should return true.

```csharp
var certificatePaths = new List<PathRepresentation>();

await DirectoryManager.IterateContentsAsync(
    new IterateDirectoryContentsRequest
    {
        Directory = "tim-berners-lee/".AsFilesystemPath(),
        Action = async path =>
        {
            certificatePaths.Add(path);

            // Stop the iterations once this delegate finishes executing.
            if (certificatePaths.Length > 10)
            {
                return false;
            }

            return true;
        }
    },
    "certificates",
    cancellationToken
);
```

### Listing the entire contents of a directory

In a similar vain to iterating over the contents of a directory, you can alternatively list the contents and perform
your required actions after by calling the `DirectoryManager.ListContentsAsync` method. This method accepts a request
that contains the directory to list the contents for.

An exception will be thrown if the directory to list the contents for does not exist.

> [!WARNING]
> If you have a particularly large directory, you may find that this method takes a long time to return. This is especially true on non-local adapters such as S3 where the contents need to be retrieved in batches of a limited number.

### Moving a directory

You can move a directory by calling the `DirectoryManager.MoveAsync` method. It takes a request containing the
source directory to move and the destination directory to move it to and returns a response containing the destination
directory path.

An exception will be thrown if the source directory does not exist or the destination directory already exists.

## Files

Files are managed by the `IFileManager` interface and/or its concrete implementation `FileManager`. File related methods
are quick, and you usually do not need to think about performance issues as they are performing operations on a single
path.

### Checking whether a file exists

To check whether a file exists you can call the `FileManager.ExistsAsync` method with a request containing the file to
check. The response returned will indicate the existence of the file.

No exceptions will be thrown if the file does or does not exist.

### Copying a file

To copy a file you can call the `FileManager.CopyAsync` method. This method accepts a request containing the source
file path and a destination path to copy it to.

An exception will be thrown if the source path does not exist or the destination path does.

### Deleting a file

You can delete a file by calling the `FileManager.DeleteAsync` method and specifying the path to delete as part of the
request.

An exception will be thrown if the path does not exist.

### Getting a file's information

To retrieve a file's information (for example its path, its size, etc) you can call the `FileManager.GetAsync` method.
This method accepts a request containing a path to a file to retrieve the information for.

An exception will be thrown if the path does not exist.

> [!WARNING]
> This method does NOT retrieve a file's contents. Use one of the 'read' methods for that.

### Getting the public URL for a file

Assuming the store's adapter provides a way to retrieve public URLs, you can retrieve one by calling the
`GetPublicUrlAsync` method, passing the path to retrieve the public URL for and an optional (and not always supported)
expiry date for the URL.

An exception will be thrown if the path does not exist.

### Moving a file

To move a file from one location to another you can call the `FileManager.MoveAsync` method and specify a source file
path and destination file path in the request.

An exception will be thrown if the source path does not exist or the destination path does.

### Reading a file's contents as a stream

You can retrieve a file's contents as a `Stream` implementation by calling the `FileManager.ReadAsStreamAsync` method
and specifying a path to read as part of its request.

An exception will be thrown if the file does not exist.

It is likely that the file's contents will be read into a `MemoryStream` from the adapter's provider SDK `Stream`
implementation prior to the stream being returned. This is to provide a consistent API layer, but may impact performance.
If you need to read parts of files at a time (if they're extremely large, for example), you'd be better off using the
adapter's SDK directly.

### Reading a file's contents as a string

As well as reading file contents as a stream you can simply read them as a string by calling the
`FileManager.ReadAsStringAsync`. This method accepts a path to a file which should have its contents read.

An exception will be thrown if the file does not exist.

### Touching (creating) a file

You can touch a file (that is, create an empty one) by calling the `FileManager.TouchAsync` method. This method accepts
a request which contains a file path that should be created.

An exception will be thrown if the file already exists.

### Writing text to a file

You can write text to a file by calling the `FileManager.WriteTextAsync` method and passing the text, the content type
and the path to write the text to as part of the request.

This method will overwrite the file if it already exists.

### Writing a stream to a file

You can write a stream to a file by calling the `FileManager.WriteStreamAsync` method and passing the text, the content
type and the path to write the stream to as part of the request.

This method will overwrite the file if it already exists.
