using System;
using Microsoft.Extensions.Configuration;
using NanoFabric.Core;

namespace NanoFabric.AspNetCore.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddNanoFabricKeyValues(this IConfigurationBuilder builder, Func<IRegistryHost> registryHostFactory)
        {
            builder.Add(new NanoFabricConfigurationSource(registryHostFactory));
            return builder;
        }
    }
}
