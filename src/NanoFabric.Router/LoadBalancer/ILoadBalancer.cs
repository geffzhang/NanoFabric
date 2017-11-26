using NanoFabric.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NanoFabric.Router
{
    public interface ILoadBalancer
    {
        Task<RegistryInformation> Endpoint(CancellationToken ct = default(CancellationToken));
    }
}
