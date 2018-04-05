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
using Butterfly.Client.AspNetCore;
using NanoFabric.AspNetCore;
using System.IO;
using IdentityServer4.AccessTokenValidation;

namespace NanoFabric.Ocelot
{
    public class Startup
    {
        /// <summary>
        /// 构造函数，初始化配置信息
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 系统配置
        /// </summary>
        public IConfiguration Configuration { get; }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Action<CacheManager.Core.ConfigurationBuilderCachePart> settings = (x) =>
            {
                x.WithMicrosoftLogging(log =>
                {
                    log.AddConsole(LogLevel.Debug);
                })
                .WithDictionaryHandle();
            };
            var authority = Configuration.GetValue<string>("Authority");

            var authenticationProviderKey = "TestKey";
            Action<IdentityServerAuthenticationOptions> options = o =>
            {
                o.Authority = authority;
                o.ApiName = "api";
                o.SupportedTokens = SupportedTokens.Both;
                o.ApiSecret = "secret";
            };
            services.AddAuthentication()
            //.AddJwtBearer("TestKey", x =>
            //{
            //    x.Authority = "test";
            //    x.Audience = "test";
            //});
            .AddIdentityServerAuthentication(authenticationProviderKey, options);

            var collectorUrl = Configuration.GetValue<string>("Butterfly:CollectorUrl");
            services.AddOcelot()
                .AddStoreOcelotConfigurationInConsul()
                .AddCacheManager(settings)
                .AddOpenTracing(option =>
                {
                    option.CollectorUrl = collectorUrl;
                    option.Service = "NanoFabric_Ocelot";
                    option.IgnoredRoutesRegexPatterns = new string[] { "/administration/status" };
                })
                .AddAdministration("/administration", "secret"); 

            services.AddNanoFabricConsul(Configuration);
            var metrics = AppMetrics.CreateDefaultBuilder()
                       .Build();
            services.AddMetrics(metrics);
            services.AddMetricsTrackingMiddleware();
            services.AddMetricsEndpoints();
            services.AddMetricsReportScheduler();
        }

        // http://edi.wang/post/2017/11/1/use-nlog-aspnet-20
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {        
            env.ConfigureNLog($"{env.ContentRootPath}{ Path.DirectorySeparatorChar}nlog.config");
            //add NLog to ASP.NET Core
            loggerFactory.AddNLog();
            var logger = loggerFactory.CreateLogger<Startup>();
            logger.LogInformation("Application - Configure is invoked");
            app.UseConsulRegisterService(Configuration);
            app.UseMetricsAllMiddleware();
            app.UseMetricsAllEndpoints();     
            app.UseOcelot().Wait();
           
        }
    }
}
