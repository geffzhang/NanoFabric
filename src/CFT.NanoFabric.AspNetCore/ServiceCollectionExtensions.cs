using System;
using Microsoft.Extensions.DependencyInjection;
using CFT.NanoFabric.Core;

namespace CFT.NanoFabric.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNanoFabric(this IServiceCollection services, Func<IRegistryHost> registryHostFactory)
        {
            var registryHost = registryHostFactory();
            var serviceRegistry = new ServiceRegistry(registryHost);
            services.AddSingleton(serviceRegistry);

            return services;
        }
    }
}
