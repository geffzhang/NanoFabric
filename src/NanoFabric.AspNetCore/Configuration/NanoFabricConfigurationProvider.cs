using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using NanoFabric.Core;

namespace NanoFabric.AspNetCore.Configuration
{
    public class NanoFabricConfigurationProvider : IConfigurationProvider
    {
        private readonly ConfigurationReloadToken _configurationReloadToken = new ConfigurationReloadToken();
        private readonly ServiceRegistry _serviceRegistry;

        public NanoFabricConfigurationProvider(Func<IRegistryHost> registryHostFactory)
        {
            if (registryHostFactory == null)
            {
                throw new ArgumentNullException(nameof(registryHostFactory));
            }

            var registryHost = registryHostFactory();
            _serviceRegistry = new ServiceRegistry(registryHost);
        }

        public bool TryGet(string key, out string value)
        {
            value = _serviceRegistry.KeyValueGetAsync(key)
                .Result;
            return value != null;
        }

        public void Set(string key, string value)
        {
            _serviceRegistry.KeyValuePutAsync(key, value)
                .Wait();
        }

        public IChangeToken GetReloadToken() => _configurationReloadToken;

        public void Load()
        {
            // requests are always up-to-date
        }

        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            return _serviceRegistry.KeyValuesGetKeysAsync(parentPath)
                .Result ?? Enumerable.Empty<string>();
        }
    }
}
