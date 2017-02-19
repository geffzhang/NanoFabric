using Consul;
using System;

namespace NanoFabric.RegistryHost.ConsulRegistry
{
    public static class HealthCheckExtensions
    {
        public static bool NeedsStatusCheck(this HealthCheck healthCheck)
        {
            if (healthCheck == null)
            {
                return false;
            }

            string serviceName = string.IsNullOrWhiteSpace(healthCheck.ServiceName) ? "" : $" {healthCheck.ServiceName}";

            // don't check consul
            if (healthCheck.ServiceName == "consul")
            {
                return false;
            }

            // don't check services without service ID
            if (healthCheck.ServiceID == "")
            {
                return false;
            }

            // don't check serfHealth
            if (healthCheck.CheckID.Equals("serfHealth", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // don't check nodes in maintenance
            if (healthCheck.CheckID.Equals("_node_maintenance", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // don't check services in maintenance
            if (healthCheck.CheckID.StartsWith("_service_maintenance:", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
