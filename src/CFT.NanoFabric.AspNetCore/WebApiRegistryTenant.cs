using CFT.NanoFabric.Core;
using System;

namespace CFT.NanoFabric.WebApi
{
    public class WebApiRegistryTenant : IRegistryTenant
    {
        public Uri Uri { get; }

        public WebApiRegistryTenant(Uri uri)
        {
            Uri = uri;
        }
    }
}
