using App.Metrics;
using Butterfly.Client.AspNetCore;
using CacheManager.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Ocelot.DependencyInjection;
using Rafty.Infrastructure;
using System;
using System.IO;

namespace NanoFabric.Ocelot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostingconfig = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hosting.json", optional: true)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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
                    config.AddJsonFile("configuration.json");
                    config.AddJsonFile("peers.json");
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices(services =>
                {
                    Action<CacheManager.Core.ConfigurationBuilderCachePart> settings = (x) =>
                    {
                        x.WithMicrosoftLogging(log =>
                        {
                            log.AddConsole(LogLevel.Debug);
                        })
                        .WithDictionaryHandle();
                    };

                    services.AddAuthentication()
                        .AddJwtBearer("TestKey", x =>
                        {
                            x.Authority = "test";
                            x.Audience = "test";
                        });

                    services.AddOcelot()
                        .AddCacheManager(settings)
                        .AddAdministration("/administration", "secret")
                        .AddRafty();


                    var metrics = AppMetrics.CreateDefaultBuilder()
                               .Build();

                    services.AddMetrics(metrics);
                    services.AddMetricsTrackingMiddleware();
                    services.AddMetricsEndpoints();
                    services.AddMetricsReportScheduler();

                    var collectorUrl = hostingconfig.GetValue<string>("Butterfly:CollectorUrl");

                    services.AddButterfly(option =>
                    {
                        option.CollectorUrl = collectorUrl;
                        option.Service = "Ocelot";
                    });
                })
                 .ConfigureLogging((hostingContext, logging) =>
                 {
                     logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                     logging.AddConsole();
                     logging.AddDebug();
                 })
                .UseIISIntegration()
                .UseMetricsWebTracking()
                .UseMetricsEndpoints()
                .UseNLog()
                .UseStartup<Startup>();
            var host = builder.Build();
            host.Run();          
        }
    }
}
