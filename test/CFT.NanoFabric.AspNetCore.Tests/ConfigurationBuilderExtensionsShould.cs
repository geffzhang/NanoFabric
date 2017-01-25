using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Xunit;
using CFT.NanoFabric.Core;
using CFT.NanoFabric.RegistryHost.ConsulRegistry;
using CFT.NanoFabric.AspNetCore.Configuration;


namespace CFT.NanoFabric.AspNetCore.Tests
{
    public class ConfigurationBuilderExtensionsShould
    {
        private readonly IRegistryHost _inMemoryRegistryHost;
        private readonly IRegistryHost _consulRegistryHost;

        public ConfigurationBuilderExtensionsShould()
        {
            _inMemoryRegistryHost = GetInMemoryRegistryHostAsync().Result;
            _consulRegistryHost = GetConsulRegistryHostAsync().Result;
        }

        private async Task<IRegistryHost> GetInMemoryRegistryHostAsync()
        {
            var registryHost = new InMemoryRegistryHost();
            await registryHost.KeyValuePutAsync("key1", "value1");
            await registryHost.KeyValuePutAsync("key2", "value2");
            await registryHost.KeyValuePutAsync("folder/key3", "value3");
            await registryHost.KeyValuePutAsync("folder/key4", "value4");

            return registryHost;
        }

        private async Task<IRegistryHost> GetConsulRegistryHostAsync()
        {
            var configuration = new ConsulRegistryHostConfiguration() { HostName = "10.125.30.152" };

            var registryHost = new ConsulRegistryHost(configuration) ;
            await registryHost.KeyValuePutAsync("key1", "value1");
            await registryHost.KeyValuePutAsync("key2", "value2");
            await registryHost.KeyValuePutAsync("folder/key3", "value3");
            await registryHost.KeyValuePutAsync("folder/key4", "value4");

            return registryHost;
        }

        private void MakeKeyValuesAvailable(IRegistryHost registryHost, string key, string expectedValue)
        {
            var config = new ConfigurationBuilder()
                .AddNanoFabricKeyValues(() => registryHost)
                .Build();

            string value = new WebHostBuilder()
                .UseConfiguration(config)
                .GetSetting(key);

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData("key1", "value1")]
        [InlineData("key2", "value2")]
        [InlineData("folder/key3", "value3")]
        [InlineData("folder/key4", "value4")]
        public void MakeKeyValuesAvailable(string key, string expectedValue)
        {
            MakeKeyValuesAvailable(_inMemoryRegistryHost, key, expectedValue);
            MakeKeyValuesAvailable(_consulRegistryHost, key, expectedValue);
        }
    }
}
