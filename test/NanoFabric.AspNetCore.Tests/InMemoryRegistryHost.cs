using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SemVer;
using NanoFabric.Core;

namespace NanoFabric.AspNetCore.Tests
{
    public class InMemoryRegistryHost : Core.IRegistryHost
    {

        private readonly List<Core.RegistryInformation> _serviceInstances = new List<RegistryInformation>();

        public IList<RegistryInformation> ServiceInstances
        {
            get { return _serviceInstances; }
            set
            {
                foreach (var registryInformation in value)
                {
                    string url = registryInformation.Address;
                    if (registryInformation.Port >= 0)
                    {
                        url += $":{registryInformation.Port}";
                    }
                    RegisterServiceAsync(registryInformation.Name, registryInformation.Version, new Uri(url), tags: registryInformation.Tags);
                }
            }
        }

        private Task<IDictionary<string, string[]>> GetServicesCatalogAsync()
        {
            IDictionary<string, string[]> results = ServiceInstances
                .GroupBy(x => x.Name, x => x.Tags)
                .ToDictionary(g => g.Key, g => g.SelectMany(x => x ?? Enumerable.Empty<string>()).ToArray());

            return Task.FromResult(results);
        }

        public Task<IList<RegistryInformation>> FindServiceInstancesAsync()
        {
            return Task.FromResult(ServiceInstances);
        }

        public async Task<IList<RegistryInformation>> FindServiceInstancesAsync(string name)
        {
            var instances = await FindServiceInstancesAsync();
            return instances.Where(x => x.Name == name).ToList();
        }

        public async Task<IList<RegistryInformation>> FindServiceInstancesWithVersionAsync(string name, string version)
        {
            var instances = await FindServiceInstancesAsync(name);
            var range = new Range(version);

            return instances.Where(x => range.IsSatisfied(x.Version)).ToList();
        }

        public async Task<IList<RegistryInformation>> FindServiceInstancesAsync(Predicate<KeyValuePair<string, string[]>> nameTagsPredicate, Predicate<RegistryInformation> registryInformationPredicate)
        {
            return (await GetServicesCatalogAsync())
                .Where(kvp => nameTagsPredicate(kvp))
                .Select(kvp => kvp.Key)
                .Select(FindServiceInstancesAsync)
                .SelectMany(task => task.Result)
                .Where(x => registryInformationPredicate(x))
                .ToList();
        }

        public async Task<IList<RegistryInformation>> FindServiceInstancesAsync(Predicate<KeyValuePair<string, string[]>> predicate)
        {
            return await FindServiceInstancesAsync(nameTagsPredicate: predicate, registryInformationPredicate: x => true);
        }

        public async Task<IList<RegistryInformation>> FindServiceInstancesAsync(Predicate<RegistryInformation> predicate)
        {
            return await FindServiceInstancesAsync(nameTagsPredicate: x => true, registryInformationPredicate: predicate);
        }

        public Task<IList<RegistryInformation>> FindAllServicesAsync()
        {
            return Task.FromResult(ServiceInstances);
        }

        public Task<RegistryInformation> RegisterServiceAsync(string serviceName, string version, Uri uri, Uri healthCheckUri = null, IEnumerable<string> tags = null)
        {
            var registryInformation = new RegistryInformation
            {
                Name = serviceName,
                Id = Guid.NewGuid().ToString(),
                Address = uri.Host,
                Port = uri.Port,
                Version = version,
                Tags = tags ?? Enumerable.Empty<string>()
            };
            ServiceInstances.Add(registryInformation);
            return Task.FromResult(registryInformation);
        }

        public async Task<bool> DeregisterServiceAsync(string serviceId)
        {
            var instance = (await FindServiceInstancesAsync()).FirstOrDefault(x => x.Id == serviceId);
            if (instance != null)
            {
                ServiceInstances.Remove(instance);
                return true;
            }

            return false;
        }

        public Task<string> RegisterHealthCheckAsync(string serviceName, string serviceId, Uri checkUri, TimeSpan? interval = null, string notes = null)
        {
            return null;
        }

        public Task<bool> DeregisterHealthCheckAsync(string checkId)
        {
            return Task.FromResult(false);
        }      
    }
}
