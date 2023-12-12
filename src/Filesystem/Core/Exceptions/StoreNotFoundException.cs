namespace LSymds.Filesystem;

/// <summary>
/// Thrown when a store is not found by a specified store name.
/// </summary>
public class StoreNotFoundException : FilesystemException
{
    /// <summary>
    /// Gets the name of the store that could not be found.
    /// </summary>
    public string StoreName { get; }

    /// <summary>
    /// Initialises a new instance of the <see cref="StoreNotFoundException" /> class referencing the store
    /// name that could not be found.
    /// </summary>
    /// <param name="storeName">The name of the store that could not be found.</param>
    public StoreNotFoundException(string storeName)
        : base($"The store '{storeName}' was not found. Have you registered it?")
    {
        StoreName = storeName;
    }
}
