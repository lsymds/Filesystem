using System.Collections.Generic;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Builder class containing properties required to fluently add the Baseline.Filesystem project to a service
    /// collection.
    /// </summary>
    public class BaselineFilesystemBuilder
    {
        /// <summary>
        /// Gets the collection of adapter registrations to register in the service collection.
        /// </summary>
        internal List<AdapterRegistrationBuilder> AdapterRegistrations { get; } = 
            new List<AdapterRegistrationBuilder>();
    }
}
