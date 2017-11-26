using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NanoFabric.Router.Tests
{
    public class ConsulPreparedQueryServiceSubscriberTests
    {
        [Fact]
        public async Task Endpoints_withoutData_returnsEmptyList()
        {
            var fixture = new ConsulPreparedQueryServiceSubscriberFixture();
            fixture.ServiceName = Guid.NewGuid().ToString();
            fixture.ClientQueryResult = new QueryResult<PreparedQueryExecuteResponse>();
            fixture.ClientQueryResult.Response = new PreparedQueryExecuteResponse
            {
                Nodes = new ServiceEntry[0]
            };

            fixture.SetPreparedQueryEndpoint();
            var subscriber = fixture.CreateSut();

            var actual = await subscriber.Endpoints();
            Assert.NotNull(actual);
            Assert.Empty(actual);
        }

        [Fact]
        public async Task Endpoints_withLotsOfData_returnsList()
        {
            var fixture = new ConsulPreparedQueryServiceSubscriberFixture();
            fixture.ServiceName = Guid.NewGuid().ToString();

            var services = new List<ServiceEntry>();
            for (var i = 0; i < 5; i++)
            {
                services.Add(new ServiceEntry
                {
                    Node = new Node
                    {
                        Address = Guid.NewGuid().ToString()
                    },
                    Service = new AgentService
                    {
                        Address = Guid.NewGuid().ToString(),
                        Port = 123
                    }
                });
            }

            fixture.ClientQueryResult = new QueryResult<PreparedQueryExecuteResponse>
            {
                Response = new PreparedQueryExecuteResponse
                {
                    Nodes = services.ToArray()
                }
            };

            fixture.SetPreparedQueryEndpoint();

            var subscriber = fixture.CreateSut();
            var actual = await subscriber.Endpoints();

            Assert.NotNull(actual);
            Assert.Equal(services.Count, actual.Count);
        }
    }
}