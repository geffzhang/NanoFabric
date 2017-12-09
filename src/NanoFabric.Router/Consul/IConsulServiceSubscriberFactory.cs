using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Router.Consul
{
    public interface IConsulServiceSubscriberFactory
    {
        IServiceSubscriber CreateSubscriber(string serviceName, ConsulSubscriberOptions consulOptions, bool watch);
    }
}
