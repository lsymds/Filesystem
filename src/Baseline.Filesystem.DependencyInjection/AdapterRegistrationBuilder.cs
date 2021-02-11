using System;
using System.Runtime.Serialization;

namespace Baseline.Filesystem.DependencyInjection
{
    public class AdapterRegistrationBuilder
    {
        internal string Name { get; set; }
        internal string RootPath { get; set; }
        internal Func<IServiceProvider, IAdapter> Resolver { get; set; }
    }

    public static class AdapterRegistrationBuilderExtensions
    {
        public static AdapterRegistrationBuilder WithName(this AdapterRegistrationBuilder builder, string name)
        {
            builder.Name = name;
            return builder;
        }

        public static AdapterRegistrationBuilder WithRootPath(this AdapterRegistrationBuilder builder, string rootPath)
        {
            builder.RootPath = rootPath;
            return builder;
        }

        public static AdapterRegistrationBuilder WithAdapter(
            this AdapterRegistrationBuilder builder,
            Func<IServiceProvider, IAdapter> resolver
        )
        {
            builder.Resolver = resolver;
            return builder;
        }
    }
}
