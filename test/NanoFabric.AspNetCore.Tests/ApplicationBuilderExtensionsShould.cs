using NanoFabric.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace NanoFabric.AspNetCore.Tests
{
    public class ApplicationBuilderExtensionsShould
    {
        [Fact]
        public void AddTenant()
        {
            var registryHost = new InMemoryRegistryHost();
            var hostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddNanoFabric(() => registryHost);
                })
                .Configure(app =>
                {
                    app.AddTenant(nameof(ApplicationBuilderExtensionsShould), "1.0.0", new Uri("http://localhost:1234"));

                    var serviceRegistry = app.ApplicationServices.GetService<ServiceRegistry>();
                    Assert.NotNull(serviceRegistry);

                    var instances = serviceRegistry.FindAllServicesAsync().Result;
                    Assert.Equal(1, instances.Count);
                });

            using (new TestServer(hostBuilder))
            {
                // ConfigureServices
                // Configure
            }
        }
    }
}
