using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Router.Consul
{
    public interface IConsulPreparedQueryServiceSubscriberFactory
    {
        IServiceSubscriber CreateSubscriber(string queryName);
    }
}
