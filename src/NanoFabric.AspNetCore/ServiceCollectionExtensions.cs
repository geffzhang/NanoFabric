using Consul;
using DnsClient;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NanoFabric.Core;
using NanoFabric.RegistryHost.ConsulRegistry;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace NanoFabric.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomIdentity(
           this IServiceCollection services,
           IApiInfo apiInfo
           )
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = apiInfo.AuthenticationAuthority;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = apiInfo.ApiName;
                    options.ApiSecret = apiInfo.ApiSecret;
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, HttpContextUser>();

            return services;
        }

        public static IServiceCollection AddNanoFabricConsul(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ConsulServiceDiscoveryOption>(configuration.GetSection("ServiceDiscovery"));
            services.RegisterConsulClient();
            services.RegisterDnsLookup();
            ConsulServiceDiscoveryOption serviceDiscoveryOption = new ConsulServiceDiscoveryOption(); 
            configuration.GetSection("ServiceDiscovery").Bind(serviceDiscoveryOption);
            services.AddNanoFabric(() => new ConsulRegistryHost(serviceDiscoveryOption.Consul));
            return services;
        }

        public static IServiceCollection AddNanoFabricConsul(this IServiceCollection services, ConsulRegistryHostConfiguration consulRegistryConfiguration)
        {
            services.AddNanoFabric(() => new ConsulRegistryHost(consulRegistryConfiguration));
            return services;
        }      

        public static IServiceCollection AddNanoFabric(this IServiceCollection services, Func<IRegistryHost> registryHostFactory)
        {
            var registryHost = registryHostFactory();
            var serviceRegistry = new ServiceRegistry(registryHost);
            services.AddSingleton(serviceRegistry);
            services.AddTransient<IStartupFilter, NanoStartupFilter>();
            return services;
        }

        private static IServiceCollection RegisterDnsLookup(this IServiceCollection services)
        {
            //implement the dns lookup and register to service container
            services.TryAddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ConsulServiceDiscoveryOption>>().Value;

                var client = new LookupClient(IPAddress.Parse("127.0.0.1"), 8600);
                if (serviceConfiguration.Consul.DnsEndpoint != null)
                {
                    client = new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
                }
                client.EnableAuditTrail = false;
                client.UseCache = true;
                client.MinimumCacheTimeout = TimeSpan.FromSeconds(1);
                return client;
            });
            return services;
        }

        /// <summary>
        /// /implement the consulclient and add to service container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static IServiceCollection RegisterConsulClient(this IServiceCollection services)
        {
            services.TryAddSingleton<IConsulClient>(p => new ConsulClient(config =>
            {
                config.Address = new Uri("http://127.0.0.1:8500");
                var serviceConfiguration = p.GetRequiredService<IOptions<ConsulServiceDiscoveryOption>>().Value;
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    config.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));

            return services;
        }

        public static IServiceCollection AddPermissiveCors(
           this IServiceCollection services
       )
        {
            services.AddCors(options =>
           {
               options.AddPolicy("PermissiveCorsPolicy", builder => builder
                   .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials()
               );
           });
            return services;
        }
    }
}
