using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MagicOnion;
using MagicOnion.Client;
using NanoFabric.Router;
using NanoFabric.Router.Consul;

namespace NanoFabric.Grpc.Client
{
    public class GRpcConnection : IGRpcConnection
    {
        private IServiceSubscriberFactory _subscriberFactory;
        private IGrpcChannelFactory _grpcChannelFactory;


        public GRpcConnection(IServiceSubscriberFactory subscriberFactory, IGrpcChannelFactory grpcChannelFactory)
        {
            this._subscriberFactory = subscriberFactory;
            this._grpcChannelFactory = grpcChannelFactory;
        }

        public async Task<TService> GetRemoteService<TService>(string serviceName) where TService : IService<TService>
        {
            var serviceSubscriber = _subscriberFactory.CreateSubscriber(serviceName, ConsulSubscriberOptions.Default, new NanoFabric.Router.Throttle.ThrottleSubscriberOptions()
            {
                MaxUpdatesPeriod = TimeSpan.FromSeconds(30),
                MaxUpdatesPerPeriod = 20
            });
            await serviceSubscriber.StartSubscription();
            serviceSubscriber.EndpointsChanged += async (sender, eventArgs) =>
            {
                var endpoints = await serviceSubscriber.Endpoints();
                var servicesInfo = string.Join(",", endpoints);
            };
            ILoadBalancer loadBalancer = new RoundRobinLoadBalancer(serviceSubscriber);
            var endPoint =  await loadBalancer.Endpoint();

            var serviceChannel = _grpcChannelFactory.Get(endPoint.Address, endPoint.Port);

            return MagicOnionClient.Create<TService>(serviceChannel);
        }
    }
}
