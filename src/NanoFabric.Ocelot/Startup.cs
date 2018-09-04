using App.Metrics;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoFabric.AspNetCore;
using NLog.Extensions.Logging;
using NLog.Web;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using Ocelot.Cache.CacheManager;
using System;
using System.IO;
using Ocelot.Administration;
using SkyWalking.AspNetCore;
using NanoFabric.AppMetrics;

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
            var authority = Configuration.GetValue<string>("Authority");
            var collectorUrl = Configuration.GetValue<string>("Skywalking:CollectorUrl");

 
            Action<IdentityServerAuthenticationOptions> options = o =>
            {
                o.Authority = authority;
                o.ApiName = "api";
                o.SupportedTokens = SupportedTokens.Both;
                o.ApiSecret = "secret";
            };

            var authenticationProviderKey = "apikey";
            Action<IdentityServerAuthenticationOptions> options2 = o =>
            {
                o.Authority = "http://localhost:5000";
                o.ApiName = "api1";
                o.RequireHttpsMetadata = false;
            };

            services.AddAuthentication()
            .AddIdentityServerAuthentication(authenticationProviderKey, options2);

            services.AddOcelot()
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                })
                 .AddConsul()
                 .AddConfigStoredInConsul()
                 .AddPolly()
                 .AddAdministration("/administration", options);

            services.AddNanoFabricConsul(Configuration);

            services.AddAppMetrics(x=>
            {
                var opt= Configuration.GetSection("AppMetrics").Get<AppMetricsOptions>();
                x.App = opt.App ;
                x.ConnectionString = opt.ConnectionString;
                x.DataBaseName = opt.DataBaseName;
                x.Env = opt.Env;
                x.Password = opt.Password;
                x.UserName = opt.UserName;
            });

            //services.AddSkyWalking(option =>
            //{
            //    option.DirectServers = collectorUrl;
            //    option.ApplicationCode = "nanofabric_ocelot";
            //});
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
     
            app.UseOcelot().Wait();
            app.UseAppMetrics();
           
        }
    }
}
