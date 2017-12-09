using Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Router.Consul
{
    public class ConsulPreparedQueryServiceSubscriberFactory : IConsulPreparedQueryServiceSubscriberFactory
    {
        private readonly IConsulClient _consulClient;

        public ConsulPreparedQueryServiceSubscriberFactory(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        public IServiceSubscriber CreateSubscriber(string queryName)
        {
            return new ConsulPreparedQueryServiceSubscriber(_consulClient, queryName);
        }
    }
}
