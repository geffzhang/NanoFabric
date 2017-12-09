using Consul;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NanoFabric.Router.Tests
{
    public class ConsulServiceSubscriberTests
    {
        [Fact]
        public async Task Endpoints_withoutData_returnsEmptyList()
        {
            var fixture = new ConsulServiceSubscriberFixture();
            fixture.ServiceName = Guid.NewGuid().ToString();
            fixture.ClientQueryResult = new QueryResult<ServiceEntry[]>();
            fixture.ClientQueryResult.Response = new ServiceEntry[0];

            fixture.SetHealthEndpoint();
            var subscriber = fixture.CreateSut();

            var actual = await subscriber.Endpoints();
            Assert.NotNull(actual);
            Assert.Empty(actual);
        }

        [Fact]
        public async Task Endpoints_withLotsOfData_returnsList()
        {
            var fixture = new ConsulServiceSubscriberFixture();
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

            fixture.ClientQueryResult = new QueryResult<ServiceEntry[]>
            {
                Response = services.ToArray()
            };

            fixture.SetHealthEndpoint();

            var subscriber = fixture.CreateSut();
            var actual = await subscriber.Endpoints();

            Assert.NotNull(actual);
            Assert.Equal(services.Count, actual.Count);
        }

        [Fact]
        public async Task Endpoints_withMultipleTags_callsConsulWithFirstTagOnly()
        {
            var fixture = new ConsulServiceSubscriberFixture();
            fixture.ServiceName = Guid.NewGuid().ToString();

            var services = new List<ServiceEntry>
            {
                new ServiceEntry
                {
                    Node = new Node
                    {
                        Address = Guid.NewGuid().ToString()
                    },
                    Service = new AgentService
                    {
                        Address = Guid.NewGuid().ToString(),
                        Port = 123,
                        Tags = new string[0]
                    }
                }
            };

            fixture.ClientQueryResult = new QueryResult<ServiceEntry[]>
            {
                Response = services.ToArray()
            };

            fixture.SetHealthEndpoint();
            fixture.Tags = new List<string>();
            fixture.Tags.Add(Guid.NewGuid().ToString());
            fixture.Tags.Add(Guid.NewGuid().ToString());

            var subscriber = fixture.CreateSut();
            await subscriber.Endpoints();

            await fixture.HealthEndpoint.Received()
                .Service(Arg.Any<string>(), fixture.Tags[0],
                    Arg.Any<bool>(), Arg.Any<QueryOptions>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Endpoints_withMultipleTagsAndMultipleMatches_returnsOnlyMatchesWithAllTags()
        {
            var fixture = new ConsulServiceSubscriberFixture();
            fixture.ServiceName = Guid.NewGuid().ToString();


            fixture.Tags = new List<string>();
            fixture.Tags.Add(Guid.NewGuid().ToString());
            fixture.Tags.Add(Guid.NewGuid().ToString());

            var superSetOfTags = new List<string>(fixture.Tags);
            superSetOfTags.Add(Guid.NewGuid().ToString());

            var services = new List<ServiceEntry>
            {
                new ServiceEntry
                {
                    Node = new Node
                    {
                        Address = Guid.NewGuid().ToString()
                    },
                    Service = new AgentService
                    {
                        Address = Guid.NewGuid().ToString(),
                        Port = 123,
                        Tags = new string[0]
                    }
                },
                new ServiceEntry
                {
                    Node = new Node
                    {
                        Address = Guid.NewGuid().ToString()
                    },
                    Service = new AgentService
                    {
                        Address = Guid.NewGuid().ToString(),
                        Port = 123,
                        Tags = new[] {fixture.Tags[0]}
                    }
                },
                new ServiceEntry
                {
                    Node = new Node
                    {
                        Address = Guid.NewGuid().ToString()
                    },
                    Service = new AgentService
                    {
                        Address = Guid.NewGuid().ToString(),
                        Port = 123,
                        Tags = new[] {fixture.Tags[1]}
                    }
                },
                new ServiceEntry
                {
                    Node = new Node
                    {
                        Address = Guid.NewGuid().ToString()
                    },
                    Service = new AgentService
                    {
                        Address = Guid.NewGuid().ToString(),
                        Port = 123,
                        Tags = fixture.Tags.ToArray() // MATCH
                    }
                },
                new ServiceEntry
                {
                    Node = new Node
                    {
                        Address = Guid.NewGuid().ToString()
                    },
                    Service = new AgentService
                    {
                        Address = Guid.NewGuid().ToString(),
                        Port = 123,
                        Tags = superSetOfTags.ToArray() // MATCH
                    }
                }
            };

            fixture.ClientQueryResult = new QueryResult<ServiceEntry[]>
            {
                Response = services.ToArray()
            };

            fixture.SetHealthEndpoint();

            var subscriber = fixture.CreateSut();
            var actual = await subscriber.Endpoints();

            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count);
        }

        [Fact]
        public async Task Endpoints_withMultipleTagsAndNoMatches_returnsEmptyList()
        {
            var fixture = new ConsulServiceSubscriberFixture();
            fixture.ServiceName = Guid.NewGuid().ToString();

            fixture.ClientQueryResult = new QueryResult<ServiceEntry[]>
            {
                Response = new ServiceEntry[0]
            };

            fixture.SetHealthEndpoint();
            fixture.Tags = new List<string>();
            fixture.Tags.Add(Guid.NewGuid().ToString());
            fixture.Tags.Add(Guid.NewGuid().ToString());

            var subscriber = fixture.CreateSut();
            var actual = await subscriber.Endpoints();

            Assert.NotNull(actual);
            Assert.Empty(actual);
        }

        [Fact]
        public async Task Endpoints_withoutServiceAddressInReturnedData_buildsUriWithNodeAddressInstead()
        {
            var fixture = new ConsulServiceSubscriberFixture();
            fixture.ServiceName = Guid.NewGuid().ToString();

            var services = new List<ServiceEntry>
            {
                new ServiceEntry
                {
                    Node = new Node
                    {
                        Address = Guid.NewGuid().ToString()
                    },
                    Service = new AgentService
                    {
                        Port = 123
                    }
                }
            };

            fixture.ClientQueryResult = new QueryResult<ServiceEntry[]>
            {
                Response = services.ToArray()
            };

            fixture.SetHealthEndpoint();

            var subscriber = fixture.CreateSut();
            var actual = await subscriber.Endpoints();

            Assert.NotNull(actual);
            Assert.Single(actual);
            Assert.True(actual[0].Address == services[0].Node.Address);
        }

        [Fact]
        public async Task Endpoints_withBothServiceAddressAndAddressInReturnedData_buildsUriWithServiceAddress()
        {
            var fixture = new ConsulServiceSubscriberFixture();
            fixture.ServiceName = Guid.NewGuid().ToString();

            var services = new List<ServiceEntry>
            {
                new ServiceEntry
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
                }
            };

            fixture.ClientQueryResult = new QueryResult<ServiceEntry[]>
            {
                Response = services.ToArray()
            };

            fixture.SetHealthEndpoint();

            var subscriber = fixture.CreateSut();
            var actual = await subscriber.Endpoints();

            Assert.NotNull(actual);
            Assert.Single(actual);
            Assert.True(actual[0].Address == services[0].Service.Address);
        }

        [Fact]
        public async Task Endpoints_withWatchSetToTrue_updatesWaitIndex()
        {
            var fixture = new ConsulServiceSubscriberFixture();
            fixture.ServiceName = Guid.NewGuid().ToString();

            fixture.Watch = true;
            var expectedWatchIndex = (ulong)500;

            fixture.ClientQueryResult = new QueryResult<ServiceEntry[]>
            {
                LastIndex = expectedWatchIndex,
                Response = new ServiceEntry[0]
            };
            fixture.SetHealthEndpoint();

            var subscriber = fixture.CreateSut();
            subscriber.WaitIndex = 100;

            await subscriber.Endpoints();

            Assert.Equal(expectedWatchIndex, subscriber.WaitIndex);
        }

        [Fact]
        public async Task Endpoints_withWatchSetToFalse_doesNotUpdateWaitIndex()
        {
            var fixture = new ConsulServiceSubscriberFixture();
            fixture.ServiceName = Guid.NewGuid().ToString();

            fixture.Watch = false;
            fixture.ClientQueryResult = new QueryResult<ServiceEntry[]>
            {
                LastIndex = 500,
                Response = new ServiceEntry[0]
            };
            fixture.SetHealthEndpoint();

            var subscriber = fixture.CreateSut();
            var expectedWatchIndex = (ulong)100;
            subscriber.WaitIndex = expectedWatchIndex;

            await subscriber.Endpoints();

            Assert.Equal(expectedWatchIndex, subscriber.WaitIndex);
        }
    }
}
