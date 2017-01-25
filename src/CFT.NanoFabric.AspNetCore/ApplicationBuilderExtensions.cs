using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CFT.NanoFabric.Core;

namespace CFT.NanoFabric.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
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

        public static string AddHealthCheck(this IApplicationBuilder app, RegistryInformation registryInformation, Uri checkUri, TimeSpan? interval = null, string notes = null)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (registryInformation == null)
            {
                throw new ArgumentNullException(nameof(registryInformation));
            }

            var serviceRegistry = app.ApplicationServices.GetRequiredService<ServiceRegistry>();
            string checkId = serviceRegistry.AddHealthCheckAsync(registryInformation.Name, registryInformation.Id, checkUri, interval, notes)
                .Result;

            return checkId;
        }

        public static bool RemoveHealthCheck(this IApplicationBuilder app, string checkId)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (string.IsNullOrEmpty(checkId))
            {
                throw new ArgumentNullException(nameof(checkId));
            }

            var serviceRegistry = app.ApplicationServices.GetRequiredService<ServiceRegistry>();
            return serviceRegistry.DeregisterHealthCheckAsync(checkId)
                .Result;
        }
    }
}
