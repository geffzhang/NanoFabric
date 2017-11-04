using NanoFabric.Core;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using NanoFabric.RegistryHost.ConsulRegistry;

namespace NanoFabric.Router.Tests
{
    public class RegistryClientShould
    {
        private readonly List<RegistryInformation> _instances;

        public RegistryClientShould()
        {
            var oneDotnetOne = new RegistryInformation { Name = "one", Address = "http://10.125.32.121", Port = 8888, Version = "1.0.0", Tags = new List<string> { "key2value2", "key1value1" } };
            var oneDotnetTwo = new RegistryInformation { Name = "one", Address = "http://10.125.32.122", Port = 8889, Version = "1.0.0", Tags = new List<string> { "key2value2", "key1value1" } };
            var twoDotnetOne = new RegistryInformation { Name = "two", Address = "http://10.126.32.121", Port = 8890, Version = "2.0.0", Tags = new List<string> { "key2value2", "key1value1" } };
            var twoDotnetTwo = new RegistryInformation { Name = "two", Address = "http://10.126.32.122", Port = 8891, Version = "2.0.0", Tags = new List<string> { "prefix/values", "key1value1" } };
            var threeDotnetOne = new RegistryInformation { Name = "three", Address = "http://http://10.127.32.121", Port = 8892, Version = "3.1.0", Tags = new List<string> { "prefix/values", "key1value1" } };
            var threeDotnetTwo = new RegistryInformation { Name = "three", Address = "http://http://10.127.32.122", Port = 8893, Version = "3.2.0", Tags = new List<string> { "prefix/values", "key1value1" } };

            _instances = new List<RegistryInformation> {
                oneDotnetOne,
                oneDotnetTwo,
                twoDotnetOne,
                twoDotnetTwo,
                threeDotnetOne,
                threeDotnetTwo
            };
        }


        [Fact]
        public void FindInstances()
        {
            var uri = new Uri("http://host:8888/values/path1/path2?key=value");
            var registryClient = new RegistryClient("prefix", new RoundRobinAddressRouter());
            var results = registryClient.FindServiceInstancesAsync(uri, _instances);
            Assert.Equal(3, results.Count);
            Assert.Equal("two", results.First().Name);
        }

        private  RegistryClient BuildRegistryClient(string prefixName)
        {
            var configuration = new ConsulRegistryHostConfiguration() { HostName = "localhost" };

            var consul = new ConsulRegistryHost(configuration);

            var registryClient = new RegistryClient(prefixName, new RoundRobinAddressRouter());
            registryClient.AddRegistryHost(consul);

            return registryClient;
        }

        [Fact]
        public void FindInstancesWithConsul()
        {
            var uri = new Uri("http://localhost:9030/values");
            var registryClient = BuildRegistryClient("urlprefix-");
            var results = registryClient.FindServiceInstancesAsync(uri).Result;
            Assert.Equal(1, results.Count);
        }

        [Fact]
        public void NotFindInstances()
        {
            var uri = new Uri("http://host:8888/path/path1/path2?key=value");
            var registryClient = new RegistryClient("prefix", new RoundRobinAddressRouter());
            var results = registryClient.FindServiceInstancesAsync(uri, _instances);

            Assert.Equal(0, results.Count);
        }

    }
}
