using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SampleService.Kestrel
{
    public class Program
    {
        private const string defaultAddress = "http://localhost:9300";
        private const string addressKey = "serveraddress";

        public static void Main(string[] args)
        {
            IWebHost host = BuildWebHost(args);
            host.Run();
        }

        private static IWebHost BuildWebHost(string[] args)
        {
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", true, false)
                .AddJsonFile("appsettings.Production.json", true, false)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            if (args != null)
            {
                configurationBuilder.AddCommandLine(args);
            }
            var hostingconfig = configurationBuilder.Build();
            var url = hostingconfig[addressKey] ?? defaultAddress;

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", false, true);
                })
                .UseConfiguration(hostingconfig)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls(url)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
