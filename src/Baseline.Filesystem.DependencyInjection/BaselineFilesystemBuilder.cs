using System;
using System.Collections.Generic;

namespace Baseline.Filesystem.DependencyInjection
{
    public class BaselineFilesystemBuilder
    {
        internal List<AdapterRegistrationBuilder> AdapterRegistrations = new List<AdapterRegistrationBuilder>();
    }

    public static class BaselineFilesystemBuilderExtensions
    {
        public static BaselineFilesystemBuilder AddAdapterRegistration(
            this BaselineFilesystemBuilder builder,
            Action<AdapterRegistrationBuilder> adapterRegistrationBuilder)
        {
            var registration = new AdapterRegistrationBuilder();
            adapterRegistrationBuilder(registration);
            
            builder.AdapterRegistrations.Add(registration);

            return builder;
        }
    }
}
