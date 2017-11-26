using NanoFabric.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NanoFabric.Router.LoadBalancer
{
    public class RandomLoadBalancer : ILoadBalancer
    {
        private readonly Random _random;
        private readonly IServiceSubscriber _subscriber;

        public RandomLoadBalancer(IServiceSubscriber subscriber, int? seed = null)
        {
            _subscriber = subscriber;
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        public async Task<RegistryInformation> Endpoint(CancellationToken ct = default(CancellationToken))
        {
            var endpoints = await _subscriber.Endpoints(ct).ConfigureAwait(false);
            return endpoints.Count == 0 ? null : endpoints[_random.Next(endpoints.Count)];
        }

    }
}
