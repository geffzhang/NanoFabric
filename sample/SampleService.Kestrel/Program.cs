using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using NLog;
using Newtonsoft.Json;
using CFT.NanoFabric.RegistryHost.ConsulRegistry;
using Microsoft.Extensions.Configuration;
using CFT.NanoFabric.AspNetCore.Configuration;

namespace SampleService.Kestrel
{
    public class Program
    {
        public const int PORT = 9030;

        public static void Main(string[] args)
        {
            var log = LogManager.GetCurrentClassLogger();
            log.Debug($"Starting {typeof(Program).Namespace}");

            var appsettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText("appsettings.json"));
            var consulConfig = new ConsulRegistryHostConfiguration { HostName = appsettings.Consul.HostName, Port = appsettings.Consul.Port };
            var config = new ConfigurationBuilder()
                .AddNanoFabricKeyValues(() => new ConsulRegistryHost(consulConfig))
                .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://*:{PORT}")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
