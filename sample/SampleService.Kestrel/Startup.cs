using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NanoFabric.Core;
using NanoFabric.AspNetCore;
using Microsoft.Extensions.Options;
using NanoFabric.RegistryHost.ConsulRegistry;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Winton.Extensions.Configuration.Consul;
using System.Threading;

namespace SampleService.Kestrel
{
    public class Startup
    {
        private readonly CancellationTokenSource _consulConfigCancellationTokenSource = new CancellationTokenSource();

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                //.AddEnvironmentVariables()
                .AddConsul(
                    $"sampleservicesettings.json",
                    _consulConfigCancellationTokenSource.Token,
                    options => {
                        options.ConsulConfigurationOptions = (cco) => {
                            cco.Address = new Uri("http://localhost:8500");
                        };
                        options.Optional = true;
                        options.ReloadOnChange = true;
                        options.OnLoadException = (exceptionContext) => {
                            exceptionContext.Ignore = true;
                        };
                    })
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// 系统配置
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            var appSettings = new AppSettings();
            Configuration.Bind(appSettings);
            var consulConfig = new ConsulRegistryHostConfiguration
            {
                HostName = appSettings.Consul.HostName,
                Port = appSettings.Consul.Port
            };
            services.AddNanoFabric(() => new ConsulRegistryHost(consulConfig));
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();
            services.AddOptions();
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            var log = loggerFactory
                      .AddNLog()
                      .CreateLogger<Startup>();

            loggerFactory.ConfigureNLog("NLog.config");

            var authority = Configuration.GetValue<string>("AppSetting:IdentityServerAuthority");

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = authority,

                RequireHttpsMetadata = false,

                ApiName = "api1",

                ApiSecret = "myApiSecret"

            });
            app.UseMvc(routes =>
             {
                 routes.MapRoute(
                     name: "default",
                     template: "{controller=Home}/{action=Index}/{id?}");
             });

            // add tenant & health check
            var localAddress = DnsHelper.GetIpAddressAsync().Result;
            var uri = new Uri($"http://{localAddress}:{Program.PORT}/");
            log.LogInformation("Registering tenant at ${uri}");
            var registryInformation = app.AddTenant("values", "1.0.0-pre", uri, tags: new[] { "urlprefix-/values" });
            log.LogInformation("Registering additional health check");
             // register service & health check cleanup
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                log.LogInformation("Removing tenant & additional health check");
                app.RemoveTenant(registryInformation.Id);
                _consulConfigCancellationTokenSource.Cancel();
            });
        }
    }
}
