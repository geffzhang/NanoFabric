using NanoFabric.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NanoFabric.Router.Tests
{
    public class EndpointTests
    {
        [Fact]
        public void ToUri_WithNullScheme_ReturnsUriWithHttpScheme()
        {
            var endpoint = new RegistryInformation
            {
                Address = Guid.NewGuid().ToString(),
                Port = 123
            };

            var actual = endpoint.ToUri();

            Assert.Equal("http", actual.Scheme);
        }

        [Fact]
        public void ToString_WithHostAndPort_ReturnsHostAndPortString()
        {
            var endpoint = new RegistryInformation
            {
                Address = Guid.NewGuid().ToString(),
                Port = 123
            };

            var actual = endpoint.ToString();
            Assert.Equal($"{endpoint.Address}:{endpoint.Port}", actual);
        }
    }
}
