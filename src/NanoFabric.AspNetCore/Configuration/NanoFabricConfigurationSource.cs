using System;
using Microsoft.Extensions.Configuration;
using NanoFabric.Core;

namespace NanoFabric.AspNetCore.Configuration
{
    public class NanoFabricConfigurationSource : IConfigurationSource
    {
        private readonly Func<IRegistryHost> _registryHostFactory;

        public NanoFabricConfigurationSource(Func<IRegistryHost> registryHostFactory)
        {
            _registryHostFactory = registryHostFactory;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new NanoFabricConfigurationProvider(_registryHostFactory);
        }
    }
}
