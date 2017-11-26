using Consul;
using NanoFabric.Router.Consul;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;

namespace NanoFabric.Router.Tests
{
    public class ConsulPreparedQueryServiceSubscriberFixture
    {
        public string ServiceName { get; set; }

        public IConsulClient Client { get; set; }

        public QueryResult<PreparedQueryExecuteResponse> ClientQueryResult { get; set; }
        public IPreparedQueryEndpoint PreparedQueryEndpoint { get; set; }

        public ConsulPreparedQueryServiceSubscriberFixture()
        {
            Client = Substitute.For<IConsulClient>();
            PreparedQueryEndpoint = Substitute.For<IPreparedQueryEndpoint>();
        }

        public void SetPreparedQueryEndpoint()
        {
            PreparedQueryEndpoint.Execute(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(ClientQueryResult));

            Client.PreparedQuery.Returns(PreparedQueryEndpoint);
        }

        public ConsulPreparedQueryServiceSubscriber CreateSut()
        {
            return new ConsulPreparedQueryServiceSubscriber(Client, ServiceName);
        }
    }
}