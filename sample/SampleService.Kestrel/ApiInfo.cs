using Microsoft.Extensions.Configuration;
using NanoFabric.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SampleService.Kestrel
{
    public class ApiInfo : IApiInfo
    {
        private ApiInfo(IConfiguration config)
        {
            AuthenticationAuthority = config["Authority"];
        }

        public string AuthenticationAuthority { get; }

        public string Title => "SampleService_Kestrel Api";

        public string Version => "V1";

        public Assembly ApplicationAssembly => GetType().Assembly;

        public IDictionary<string, string> Scopes => new Dictionary<string, string>
        {
            {"api1", Title}
        };

        public SwaggerAuthInfo SwaggerAuthInfo => new SwaggerAuthInfo(
            "echoapiswaggerui", "", ""
            );

        public static IApiInfo Instantiate(IConfiguration config)
        {
            Instance = new ApiInfo(config);
            return Instance;
        }

        public static IApiInfo Instance { get; private set; }

        public string ApiName => "api1";

        public string ApiSecret => "secret";

        public string BindAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int BindPort { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
