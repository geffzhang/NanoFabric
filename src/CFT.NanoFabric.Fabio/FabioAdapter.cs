using CFT.NanoFabric.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CFT.NanoFabric.Fabio
{
    public class FabioAdapter : IResolveServiceInstances
    {
        private readonly Uri _fabioUri;

        public FabioAdapter(Uri fabioUri, string prefixName = "urlprefix-")
        {
            _fabioUri = fabioUri;
        }

        private Task<IList<RegistryInformation>> GetFabioResult(string name = "")
        {
            return Task.FromResult<IList<RegistryInformation>>(new[]
            {
                new RegistryInformation
                {
                    Name = name,
                    Address = _fabioUri.GetSchemeAndHost(),
                    Port = _fabioUri.Port
                }
            });
        }

        public Task<IList<RegistryInformation>> FindServiceInstancesAsync()
        {
            return GetFabioResult();
        }

        public Task<IList<RegistryInformation>> FindServiceInstancesAsync(string name)
        {
            return GetFabioResult(name);
        }

        public Task<IList<RegistryInformation>> FindServiceInstancesWithVersionAsync(string name, string version)
        {
            return GetFabioResult(name);
        }

        public Task<IList<RegistryInformation>> FindServiceInstancesAsync(Predicate<KeyValuePair<string, string[]>> nameTagsPredicate, Predicate<RegistryInformation> registryInformationPredicate)
        {
            return GetFabioResult();
        }

        public Task<IList<RegistryInformation>> FindServiceInstancesAsync(Predicate<KeyValuePair<string, string[]>> predicate)
        {
            return GetFabioResult();
        }

        public Task<IList<RegistryInformation>> FindServiceInstancesAsync(Predicate<RegistryInformation> predicate)
        {
            return GetFabioResult();
        }

        public Task<IList<RegistryInformation>> FindAllServicesAsync()
        {
            return GetFabioResult();
        }
    }
}
