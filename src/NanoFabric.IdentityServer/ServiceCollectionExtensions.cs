using NanoFabric.IdentityServer.Interfaces.Repositories;
using NanoFabric.IdentityServer.Interfaces.Services;
using NanoFabric.IdentityServer.Repositories.ClientAggregate.InMemory;
using NanoFabric.IdentityServer.Repositories.ResourceAggregate.InMemory;
using NanoFabric.IdentityServer.Repositories.UserAggregate.InMemory;
using NanoFabric.IdentityServer.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace NanoFabric.IdentityServer
{
    public static class ServiceCollectionExtensions
    {
        public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration, Func<TConfig> pocoProvider) where TConfig : class
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (pocoProvider == null) throw new ArgumentNullException(nameof(pocoProvider));

            var config = pocoProvider();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }

        public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration, TConfig config) where TConfig : class
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (config == null) throw new ArgumentNullException(nameof(config));

            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }
    }
}
