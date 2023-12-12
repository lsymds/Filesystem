namespace LSymds.Filesystem;

/// <summary>
/// Thrown when a store with the same normalised (lowercased) name has been registered.
/// </summary>
public class StoreAlreadyRegisteredException : BaselineFilesystemException
{
    /// <summary>
    /// The name of the store that has previously been registered.
    /// </summary>
    public string StoreName { get; }

    /// <summary>
    /// Initialises a new instance of the <see cref="StoreAlreadyRegisteredException" /> class by referencing
    /// the duplicate store name.
    /// </summary>
    /// <param name="storeName">The store name that is already registered.</param>
    public StoreAlreadyRegisteredException(string storeName)
        : base($"The store '{storeName}' is already registered.")
    {
        StoreName = storeName;
    }
}
