using Consul;
using Grpc.Core;
using MagicOnion.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoFabric.Core;
using NanoFabric.RegistryHost.ConsulRegistry;
using System;
using System.Collections.Generic;
using System.Linq;
using GRpcServer = Grpc.Core.Server;

namespace NanoFabric.Grpc.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        //the method check the service discovery parameter register ipaddress to generate service agent
        //those service agents will deregister when the app stop 
        public static IApplicationBuilder UseGrpcConsulRegisterService(this IApplicationBuilder app, IConfiguration configuration, IApiInfo apiInfo)
        {
            ConsulServiceDiscoveryOption serviceDiscoveryOption = new ConsulServiceDiscoveryOption();
            configuration.GetSection("ServiceDiscovery").Bind(serviceDiscoveryOption);
            app.UseGrpcConsulRegisterService(serviceDiscoveryOption, apiInfo);
            return app;
        }

        //the method check the service discovery parameter register ipaddress to generate service agent
        //those service agents will deregister when the app stop 
        public static IApplicationBuilder UseGrpcConsulRegisterService(this IApplicationBuilder app, ConsulServiceDiscoveryOption serviceDiscoveryOption, IApiInfo apiInfo)
        {
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>() ??
                 throw new ArgumentException("Missing Dependency", nameof(IApplicationLifetime));

            if (serviceDiscoveryOption.Consul == null)
                throw new ArgumentException("Missing Dependency", nameof(serviceDiscoveryOption.Consul));
            var consul = app.ApplicationServices.GetRequiredService<IConsulClient>() ?? throw new ArgumentException("Missing dependency", nameof(IConsulClient));

            //create logger to record the important information
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("NanoFabricServiceBuilder");

            if (string.IsNullOrEmpty(serviceDiscoveryOption.ServiceName))
                throw new ArgumentException("service name must be configure", nameof(serviceDiscoveryOption.ServiceName));
            IEnumerable<Uri> addresses = null;
            if (serviceDiscoveryOption.Endpoints != null && serviceDiscoveryOption.Endpoints.Length > 0)
            {
                logger.LogInformation($"Using {serviceDiscoveryOption.Endpoints.Length} configured endpoints for service registration");
                addresses = serviceDiscoveryOption.Endpoints.Select(p => new Uri(p));
            }
            else
            {
                logger.LogInformation($"Trying to use server.Features to figure out the service endpoint for registration.");
                var features = app.Properties["server.Features"] as FeatureCollection;
                addresses = features.Get<IServerAddressesFeature>().Addresses.Select(p => new Uri(p)).ToArray();
            }
            var grpcServer = InitializeGrpcServer(apiInfo);

            foreach (var address in addresses)
            {
                UriBuilder myUri = new UriBuilder(address.Scheme, address.Host, apiInfo.BindPort);

                var serviceID = GetServiceId(serviceDiscoveryOption.ServiceName, myUri.Uri);

                logger.LogInformation($"Registering service {serviceID} for address {address}.");
                Uri healthCheck = null;
                if (!string.IsNullOrEmpty(serviceDiscoveryOption.HealthCheckTemplate))
                {
                    healthCheck = new Uri(myUri.Uri, serviceDiscoveryOption.HealthCheckTemplate);
                    logger.LogInformation($"Adding healthcheck for {serviceID},checking {healthCheck}");
                }
                var registryInformation = app.AddTenant(serviceDiscoveryOption.ServiceName, serviceDiscoveryOption.Version, myUri.Uri, healthCheckUri: healthCheck, tags: new[] { $"urlprefix-/{serviceDiscoveryOption.ServiceName}" });
                logger.LogInformation("Registering additional health check");
                // register service & health check cleanup
                applicationLifetime.ApplicationStopping.Register(() =>
                {
                    try
                    {
                        grpcServer.ShutdownAsync().Wait();
                    }
                    catch(Exception ex)
                    {
                        logger.LogError($"grpcServer had shutown {ex}");
                    }
                    logger.LogInformation("Removing tenant & additional health check");
                    app.RemoveTenant(registryInformation.Id);
                });
            }
            return app;
        }
        
        private static string GetServiceId(string serviceName, Uri uri)
        {
            return $"GRPC_{serviceName}_{uri.Host.Replace(".", "_")}_{uri.Port}";
        }

        public static RegistryInformation AddTenant(this IApplicationBuilder app, string serviceName, string version, Uri uri, Uri healthCheckUri = null, IEnumerable<string> tags = null)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            var serviceRegistry = app.ApplicationServices.GetRequiredService<ServiceRegistry>();
            var registryInformation = serviceRegistry.RegisterServiceAsync(serviceName, version, uri, healthCheckUri, tags)
                .Result;

            return registryInformation;
        }

        public static bool RemoveTenant(this IApplicationBuilder app, string serviceId)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (string.IsNullOrEmpty(serviceId))
            {
                throw new ArgumentNullException(nameof(serviceId));
            }

            var serviceRegistry = app.ApplicationServices.GetRequiredService<ServiceRegistry>();
            return serviceRegistry.DeregisterServiceAsync(serviceId)
                .Result;
        }

        /// <summary>
        /// Initializing the GRPC service
        /// </summary>
        /// <param name="config">Grpc setting</param>
        private static GRpcServer InitializeGrpcServer(IApiInfo apiInfo)
        {
            var grpcServer = new GRpcServer
            {
                Ports = { new ServerPort(apiInfo.BindAddress, apiInfo.BindPort, ServerCredentials.Insecure) },
                Services =
                {
                    MagicOnionEngine.BuildServerServiceDefinition()
                }
            };
            grpcServer.Start();
            return grpcServer;
        }

    }
}
