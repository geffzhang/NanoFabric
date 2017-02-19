using NanoFabric.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NanoFabric.Router.Tests
{
    public class RoundRobinAddressRouterShould
    {
        private List<RegistryInformation> instances;

        [Fact]
        public void  TrampArround()
        {
            var oneDotnetOne = new RegistryInformation { Name = "one", Address = "1", Port = 8888, Version = "1.0.0" };
            var oneDotnetTwo = new RegistryInformation { Name = "one", Address = "2", Port = 8889, Version = "1.0.0"};
            var twoDotnetOne = new RegistryInformation { Name = "one", Address = "3", Port = 8890, Version = "2.0.0" };
            var twoDotnetTwo = new RegistryInformation { Name = "one", Address = "4", Port = 8891, Version = "2.0.0"};
            var threeDotnetOne = new RegistryInformation { Name = "one", Address = "5", Port = 8892, Version = "3.1.0" };
            var threeDotnetTwo = new RegistryInformation { Name = "one", Address = "6", Port = 8893, Version = "3.2.0" };

           instances = new List<RegistryInformation> {
                oneDotnetOne,
                oneDotnetTwo,
                twoDotnetOne,
                twoDotnetTwo,
                threeDotnetOne,
                threeDotnetTwo
            };

            var router = new RoundRobinAddressRouter();

            var next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("1", next.Address);

            next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("2", next.Address);

            next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("3", next.Address);

            next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("4", next.Address);

            next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("5", next.Address);

            next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("6", next.Address);

            next = router.Choose(instances);
            Assert.NotNull(next);
            Assert.Equal("1", next.Address);
        }
    }
}
