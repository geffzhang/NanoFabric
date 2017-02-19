using NanoFabric.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace NanoFabric.Router
{
    public class RegistryClient
    {
        private readonly IRouter<RegistryInformation> _router;
        private readonly List<IRegistryHost> _registryHosts = new List<IRegistryHost>();

        public string PrefixName { get; }

        public RegistryClient(string prefixName, IRouter<RegistryInformation> router)
        {
            PrefixName = prefixName;
            _router = router;
        }

        public void AddRegistryHost(IRegistryHost registryHost)
        {
            _registryHosts.Add(registryHost);
        }

        public void RemoveRegistryHost(IRegistryHost registryHost)
        {
            _registryHosts.Remove(registryHost);
        }

        public async Task<IList<RegistryInformation>> FindServiceInstancesAsync()
        {
            var allInstances = new List<RegistryInformation>();

            foreach (var registryHost in _registryHosts)
            {
                var instances = await registryHost.FindServiceInstancesAsync();
                allInstances.AddRange(instances);
            }

            return allInstances;
        }

        public IList<RegistryInformation> FindServiceInstancesAsync(Uri uri, IEnumerable<RegistryInformation> instances)
        {
            var results = instances
                .Where(x => x.Tags.Any(tag => tag.StartsWith(PrefixName, StringComparison.OrdinalIgnoreCase)
                    && uri.StartsWithSegments(tag.Substring(PrefixName.Length))))
                .ToList();

            return results;
        }

        public async Task<IList<RegistryInformation>> FindServiceInstancesAsync(Uri uri)
        {
            var instances = await FindServiceInstancesAsync();
            return FindServiceInstancesAsync(uri, instances);
        }

        public RegistryInformation Choose(IList<RegistryInformation> instances)
        {
            return _router.Choose(instances);
        }
    }
}
