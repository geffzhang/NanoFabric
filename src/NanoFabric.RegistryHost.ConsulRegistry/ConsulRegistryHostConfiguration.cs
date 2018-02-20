using System;
using System.Net;

namespace NanoFabric.RegistryHost.ConsulRegistry
{
    public class ConsulRegistryHostConfiguration
    {
        public string HttpEndpoint { get; set; }

        public DnsEndpoint DnsEndpoint { get; set; }

        public string Datacenter { get; set; }

        public string AclToken { get; set; }

        public TimeSpan? LongPollMaxWait { get; set; }

        public TimeSpan? RetryDelay { get; set; } = Defaults.ErrorRetryInterval;
    }

    public class DnsEndpoint
    {
        public string Address { get; set; }

        public int Port { get; set; }

        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }
    }

    public static class Defaults
    {
        public static TimeSpan ErrorRetryInterval => TimeSpan.FromSeconds(15);

        public static TimeSpan UpdateMaxInterval => TimeSpan.FromSeconds(15);
    }
}
