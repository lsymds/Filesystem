using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LSymds.Filesystem.Tests.Adapters;

public interface IIntegrationTestAdapter : IAsyncDisposable
{
    /// <summary>
    /// Bootstraps the adapter for the test, creating any dependent connections or pre-requisite folders that may
    /// be required.
    /// </summary>
    ValueTask<IAdapter> BootstrapAsync();

    /// <summary>
    /// Creates a file within the adapter and writes any text. Should throw an exception if the file already exists.
    /// </summary>
    ValueTask CreateFileAndWriteTextAsync(PathRepresentation path, string contents = "");

    /// <summary>
    /// Returns if there are any files or directories under a given path.
    /// </summary>
    ValueTask<bool> HasFilesOrDirectoriesUnderPathAsync(PathRepresentation path);

    /// <summary>
    /// Returns if a given path representation for a file exists within the adapter.
    /// </summary>
    ValueTask<bool> FileExistsAsync(PathRepresentation path);

    /// <summary>
    /// Returns if a given path representation for a directory exists within the adapter.
    /// </summary>
    ValueTask<bool> DirectoryExistsAsync(PathRepresentation path);

    /// <summary>
    /// Reads the contents of a file as a string.
    /// </summary>
    ValueTask<string> ReadFileAsStringAsync(PathRepresentation path);

    /// <summary>
    /// Gets the text that should be present in a public URL for a given path.
    /// </summary>
    ValueTask<IReadOnlyCollection<string>> TextThatShouldBeInPublicUrlForPathAsync(
        PathRepresentation path
    );
}
