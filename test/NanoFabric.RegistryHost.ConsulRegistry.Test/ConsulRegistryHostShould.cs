using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using NanoFabric.Core;
using NanoFabric.RegistryHost.ConsulRegistry;

namespace NanoFabric.RegistryHost.ConsulRegistry.Tests
{
    public class ConsulRegistryHostShould
    {
        private readonly IRegistryHost _registryHost;

        public ConsulRegistryHostShould()
        {
            var configuration = new ConsulRegistryHostConfiguration() {   HttpEndpoint = "http://127.0.0.1:8500" , DnsEndpoint = new DnsEndpoint() {  Address = "127.0.0.1", Port = 8600 } } ;
            _registryHost = new ConsulRegistryHost(configuration);
        }

        [Fact]
        public async Task FindServicesAsync()
        {
            var services = await _registryHost.FindServiceInstancesAsync("consul");

            Assert.NotNull(services);
            Assert.True(services.Any());
        }

        [Fact]
        public async Task RegisterServiceAsync()
        {
            var serviceName = nameof(ConsulRegistryHostShould);
            _registryHost.RegisterServiceAsync(serviceName, serviceName, new Uri("http://localhost:1234"))
                .Wait();

            Func<string, Task<RegistryInformation>> findTenant = async s => (await ((ConsulRegistryHost)_registryHost).FindAllServicesAsync())
                .FirstOrDefault(x => x.Name == s);

            var tenant = findTenant(serviceName).Result;
            Assert.NotNull(tenant);
            await _registryHost.DeregisterServiceAsync(tenant.Id);
            Assert.Null(findTenant(serviceName).Result);
        }
     
    }
}
