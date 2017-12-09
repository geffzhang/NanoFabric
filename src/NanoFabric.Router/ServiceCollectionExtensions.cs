using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NanoFabric.Router.Cache;
using NanoFabric.Router.Cache.Internal;
using NanoFabric.Router.Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Router
{
    public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsulServiceDiscovery(this IServiceCollection services, ConsulConfiguration consulConfiguration = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            string consulHost = consulConfiguration?.HostName ?? "localhost";
            int consulPort = consulConfiguration?.Port ?? 8500;

            var consul = new ConsulClient(config =>
            {
                config.Address = new Uri($"http://{consulHost}:{consulPort}");
            });

            services.TryAddSingleton<IConsulClient, ConsulClient>();
            services.AddSingleton<IConsulServiceSubscriberFactory, ConsulServiceSubscriberFactory>();
            services.AddSingleton<IConsulPreparedQueryServiceSubscriberFactory, ConsulPreparedQueryServiceSubscriberFactory>();
            return services;
        }

        public static IServiceCollection AddCacheServiceSubscriber(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddMemoryCache();
            services.TryAddSingleton<ICacheClient, CacheClient>();
            services.TryAddSingleton<ICacheServiceSubscriberFactory, CacheServiceSubscriberFactory>();
            return services;
        }
    }
}
