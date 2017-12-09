using Consul;
using NanoFabric.Router.Consul;
using NSubstitute;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NanoFabric.Router.Tests
{
    public class ConsulServiceSubscriberFixture
    {
        public string ServiceName { get; set; }
        public List<string> Tags { get; set; }
        public bool PassingOnly { get; set; }
        public bool Watch { get; set; }

        public IConsulClient Client { get; set; }
        public IServiceSubscriber ServiceSubscriber { get; set; }

        public QueryResult<ServiceEntry[]> ClientQueryResult { get; set; }
        public IHealthEndpoint HealthEndpoint { get; set; }

        public ConsulServiceSubscriberFixture()
        {
            Client = Substitute.For<IConsulClient>();
            HealthEndpoint = Substitute.For<IHealthEndpoint>();
            ServiceSubscriber = Substitute.For<IServiceSubscriber>();
        }

        public void SetHealthEndpoint()
        {
            HealthEndpoint.Service(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<QueryOptions>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(ClientQueryResult));

            Client.Health.Returns(HealthEndpoint);
        }

        public ConsulServiceSubscriber CreateSut()
        {
            return new ConsulServiceSubscriber(Client, ServiceName, Tags, PassingOnly, Watch);
        }
    }
}
