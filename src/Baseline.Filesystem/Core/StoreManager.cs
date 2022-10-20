using System.Collections.Concurrent;

namespace Baseline.Filesystem;

/// <summary>
/// Provides a way to manage stores.
/// </summary>
public class StoreManager : IStoreManager
{
    private readonly ConcurrentDictionary<string, StoreRegistration> _stores = new();

    /// <inheritdoc />
    public StoreRegistration Get(string name)
    {
        var normalisedName = NormaliseStoreName(name);

        if (!StoreAlreadyRegistered(normalisedName))
            throw new StoreNotFoundException(name);

        return _stores[normalisedName];
    }

    /// <inheritdoc />
    public void Register(StoreRegistration registration)
    {
        var normalisedName = NormaliseStoreName(registration.Name);

        if (StoreAlreadyRegistered(normalisedName))
        {
            throw new StoreAlreadyRegisteredException(normalisedName);
        }

        if (
            registration.RootPath != null
            && !registration.RootPath.FinalPathPartIsObviouslyADirectory
        )
        {
            throw new PathIsNotObviouslyADirectoryException(registration.RootPath.OriginalPath);
        }

        _stores[normalisedName] = registration;
    }

    /// <summary>
    /// Normalises and returns a given store name to ensure all store names are formatted in the same way.
    /// Currently, store names are simply lowercased.
    /// </summary>
    /// <param name="name">The original store name.</param>
    private string NormaliseStoreName(string name)
    {
        return name.ToLower();
    }

    /// <summary>
    /// Checks and returns whether a store with the given, normalised name has already been registered.
    /// </summary>
    /// <param name="name">The normalised store name to check.</param>
    private bool StoreAlreadyRegistered(string name)
    {
        return _stores.ContainsKey(name);
    }
}
