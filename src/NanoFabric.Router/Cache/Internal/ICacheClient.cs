using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Router.Cache.Internal
{
    public interface ICacheClient
    {
        T Get<T>(object key);
        T Set<T>(object key, T value);
        void Remove(object key);
    }
}
