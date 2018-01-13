using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CacheManager.Core;
using Ocelot.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Ocelot.Middleware;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;
using App.Metrics;
using Rafty.Infrastructure;
using NLog.Web;
using NLog.Extensions.Logging;

namespace NanoFabric.Ocelot
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Action<ConfigurationBuilderCachePart> settings = (x) =>
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
        }
        // http://edi.wang/post/2017/11/1/use-nlog-aspnet-20
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            env.ConfigureNLog($"{env.ContentRootPath}/nlog.config");
            //add NLog to ASP.NET Core
            loggerFactory.AddNLog();

            var logger = loggerFactory.CreateLogger<Startup>();
            logger.LogInformation("Application - Configure is invoked");

            app.UseMetricsAllMiddleware();
            app.UseMetricsAllEndpoints();
            await app.UseOcelot();
        }
    }
}
