using NanoFabric.Core;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NanoFabric.Router.Tests
{
    public class RoundRobinAddressRouterShould
    {
        [Fact]
        public async Task Endpoint_ZeroEndpoints_ReturnsNull()
        {
            var subscriber = Substitute.For<IServiceSubscriber>();
            subscriber.Endpoints().Returns(Task.FromResult(new List<RegistryInformation>()));
            var lb = new RoundRobinLoadBalancer(subscriber);

            var actual = await lb.Endpoint();

            Assert.Null(actual);
        }

        [Fact]
        public async Task Endpoint_OneEndpoint_ReturnsEndpoint()
        {
            var endpoint = new RegistryInformation {  Address = Guid.NewGuid().ToString(), Port = 123 };

            var subscriber = Substitute.For<IServiceSubscriber>();
            subscriber.Endpoints().Returns(Task.FromResult(new List<RegistryInformation> { endpoint }));
            var lb = new RoundRobinLoadBalancer(subscriber);

            var actual = await lb.Endpoint();

            Assert.Equal(endpoint.Address, actual.Address);
            Assert.Equal(endpoint.Port, actual.Port);
        }

        [Fact]
        public async Task Endpoint_MultipleEndpoints_ReturnsEndpointInOrder()
        {
            var expectedList = new List<RegistryInformation>
            {
                new RegistryInformation { Address = Guid.NewGuid().ToString(), Port = 1},
                new RegistryInformation { Address = Guid.NewGuid().ToString(), Port = 2},
                new RegistryInformation { Address = Guid.NewGuid().ToString(), Port = 3}
            };

            var subscriber = Substitute.For<IServiceSubscriber>();
            subscriber.Endpoints().Returns(Task.FromResult(expectedList));
            var lb = new RoundRobinLoadBalancer(subscriber);

            foreach (var expected in expectedList)
            {
                var actual = await lb.Endpoint();
                Assert.Equal(expected.Address, actual.Address);
                Assert.Equal(expected.Port, actual.Port);
            }

            var actualReset = await lb.Endpoint();
            Assert.Equal(expectedList[0].Address, actualReset.Address);
            Assert.Equal(expectedList[0].Port, actualReset.Port);
        }

        [Fact]
        public async Task Endpoint_ResetsNumberOfEndpoints_ReturnsEndpointAndResets()
        {
            var expectedList = new List<RegistryInformation>
            {
                new RegistryInformation { Address = Guid.NewGuid().ToString(), Port = 1},
                new RegistryInformation { Address = Guid.NewGuid().ToString(), Port = 2},
                new RegistryInformation { Address = Guid.NewGuid().ToString(), Port = 3}
            };

            var subscriber = Substitute.For<IServiceSubscriber>();
            subscriber.Endpoints().Returns(Task.FromResult(expectedList));
            var lb = new RoundRobinLoadBalancer(subscriber);

            foreach (var expected in expectedList.Take(2))
            {
                var actual = await lb.Endpoint();
                Assert.Equal(expected.Address, actual.Address );
                Assert.Equal(expected.Port, actual.Port);
            }

            subscriber.Endpoints().Returns(Task.FromResult(expectedList.Take(2).ToList()));
            var actualReset = await lb.Endpoint();
            Assert.Equal(expectedList[0].Address, actualReset.Address);
            Assert.Equal(expectedList[0].Port, actualReset.Port);
        }

    }
}
