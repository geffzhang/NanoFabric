using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using Rafty.Infrastructure;

namespace NanoFabric.Ocelot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostingconfig = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hosting.json", optional: true)
            .AddCommandLine(args)
            .Build();

            var url = hostingconfig.GetValue<string>("urls");

            IWebHostBuilder builder = new WebHostBuilder();
            builder.ConfigureServices(s => {
                s.AddSingleton(builder);
                s.AddSingleton(new NodeId(url));
            });
            builder.UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(hostingconfig)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("configuration.json");
                    config.AddJsonFile("peers.json");
                    config.AddEnvironmentVariables();
                })
                
                 .ConfigureLogging((hostingContext, logging) =>
                 {
                     logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                     logging.AddConsole();
                 })
                .UseIISIntegration()
                .UseMetricsWebTracking()
                .UseMetricsEndpoints()
                .UseStartup<Startup>();
            var host = builder.Build();
            host.Run();          
        }
    }
}
