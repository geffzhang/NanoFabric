using Consul;
using NanoFabric.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NanoFabric.Router.Consul
{
    public class ConsulPreparedQueryServiceSubscriber : IServiceSubscriber
    {
        private readonly IConsulClient _client;
        private readonly string _queryName;

        public ConsulPreparedQueryServiceSubscriber(IConsulClient client, string queryName)
        {
            _client = client;
            _queryName = queryName;
        }

        public async Task<List<RegistryInformation>> Endpoints(CancellationToken ct = default(CancellationToken))
        {
            var servicesQuery = await
               _client.PreparedQuery.Execute(_queryName, ct)
                   .ConfigureAwait(false);

            return servicesQuery.Response.Nodes.Select(service => service.ToEndpoint()).ToList();
        }

        public void Dispose() { }
    }
}