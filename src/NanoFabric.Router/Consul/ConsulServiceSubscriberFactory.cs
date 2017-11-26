using Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Router.Consul
{
    public class ConsulServiceSubscriberFactory : IConsulServiceSubscriberFactory
    {
        private readonly IConsulClient _consulClient;

        public ConsulServiceSubscriberFactory(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        public IServiceSubscriber CreateSubscriber(string serviceName, ConsulSubscriberOptions consulOptions, bool watch = false)
        {
            return new ConsulServiceSubscriber(_consulClient, serviceName, consulOptions, watch);
        }
    }
}
