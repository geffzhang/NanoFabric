using NanoFabric.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NanoFabric.AspNetCore.Tests
{
    public class ServiceCollectionExtensionsShould
    {
        private IRegistryHost GetRegistryHost()
        {
            return new InMemoryRegistryHost
            {
                ServiceInstances = new List<RegistryInformation>
                {
                    new RegistryInformation
                    {
                        Name = nameof(ServiceCollectionExtensionsShould),
                        Address = "http://0.0.0.0"
                    }
                }
            };
        }

        [Fact]
        public void AddNanoFabric()
        {
            var hostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddNanoFabric(GetRegistryHost);
                })
                .Configure(app =>
                {
                    var serviceRegistry = app.ApplicationServices.GetService<ServiceRegistry>();
                    Assert.NotNull(serviceRegistry);
                });

            using (new TestServer(hostBuilder))
            {
                // ConfigureServices
                // Configure
            }
        }

        [Fact]
        public void ResolveServiceInstances()
        {
            var hostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddNanoFabric(GetRegistryHost);
                })
                .Configure(app =>
                {
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