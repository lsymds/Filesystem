using System.Collections.Generic;

namespace LSymds.Filesystem;

/// <summary>
/// Builder class containing properties required to fluently add the Filesystem project to a service
/// collection.
/// </summary>
public class FilesystemBuilder
{
    /// <summary>
    /// Gets the collection of store registrations to register in the service collection.
    /// </summary>
    internal List<StoreRegistrationBuilder> StoreRegistrations { get; } =
        new List<StoreRegistrationBuilder>();
}
