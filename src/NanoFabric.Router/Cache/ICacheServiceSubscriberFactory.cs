using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Router.Cache
{
    public interface ICacheServiceSubscriberFactory
    {
        IPollingServiceSubscriber CreateSubscriber(IServiceSubscriber serviceSubscriber);
    }
}
