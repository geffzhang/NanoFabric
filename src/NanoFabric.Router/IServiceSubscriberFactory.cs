using NanoFabric.Router.Consul;
using NanoFabric.Router.Throttle;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Router
{
    public interface IServiceSubscriberFactory
    {
        IPollingServiceSubscriber CreateSubscriber(string serviceName);

        IPollingServiceSubscriber CreateSubscriber(string serviceName, ConsulSubscriberOptions consulOptions,
            ThrottleSubscriberOptions throttleOptions);
    }
}
