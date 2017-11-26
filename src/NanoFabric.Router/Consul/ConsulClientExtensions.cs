using Consul;
using NanoFabric.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NanoFabric.Router.Consul
{
    public static class ConsulClientExtensions
    {
        private const string VERSION_PREFIX = "version-";

        public static RegistryInformation ToEndpoint(this ServiceEntry serviceEntry)
        {
            var host = !string.IsNullOrWhiteSpace(serviceEntry.Service.Address)
                ? serviceEntry.Service.Address
                : serviceEntry.Node.Address;
            return new RegistryInformation
            {
                Name = serviceEntry.Service.Service,
                Address = host,
                Port = serviceEntry.Service.Port,
                Version = GetVersionFromStrings(serviceEntry.Service.Tags),
                Tags = serviceEntry.Service.Tags ?? Enumerable.Empty<string>(),
                Id = serviceEntry.Service.ID
            };
        }

        private  static string GetVersionFromStrings(IEnumerable<string> strings)
        {
            return strings
                ?.FirstOrDefault(x => x.StartsWith(VERSION_PREFIX, StringComparison.Ordinal))
                .TrimStart(VERSION_PREFIX);
        }
    }
}
