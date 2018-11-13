using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoFabric.AspNetCore;
using NanoFabric.AspNetCore.Cors;
using NanoFabric.Autofac;
using NanoFabric.Core;
using NanoFabric.Mediatr;
using NanoFabric.Mediatr.Autofac;
using NanoFabric.Router;
using NanoFabric.Swagger;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Threading;
using NanoFabric.AspNetCore.Middleware;
using Microsoft.AspNetCore.HttpOverrides;
using IdentityServer4.AccessTokenValidation;
using SkyWalking.AspNetCore;

namespace SampleService.Kestrel
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private readonly CancellationTokenSource _consulConfigCancellationTokenSource = new CancellationTokenSource();

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
        /// https://damienbod.com/2018/02/02/securing-an-asp-net-core-mvc-application-which-uses-a-secure-api/
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddNanoFabricConsul(Configuration);
            services.AddNanoFabricConsulRouter();

            //services.AddAuthorization();
            services.AddCors();
            services.AddDistributedMemoryCache();

            services
                .AddApplication<InMemoryRequestManager>(Configuration)
                .AddPermissiveCors()
                .AddCustomIdentity(ApiInfo.Instance)
                .AddCustomSwagger(ApiInfo.Instance);

            //var collectorUrl = Configuration.GetValue<string>("Skywalking:CollectorUrl");
            //services.AddSkyWalking(option =>
            //{
            //    option.DirectServers = collectorUrl;
            //    option.ApplicationCode = "SampleService_Kestrel";
            //});

            services.AddMvc()
               .AddMvcApiResult();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
   
            return services.ConvertToAutofac(
                MediatrModule.Create(ApiInfo.Instance.ApplicationAssembly)
                );
         
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime applicationLifetime, IApiInfo apiInfo)
        {
            loggerFactory.AddNLog();
            NLog.LogManager.LoadConfiguration("NLog.config");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app .UseDeveloperExceptionPage()
                .UsePermissiveCors()
                .UseCustomSwagger(apiInfo)
                .UseAuthentication()
                .UseMvc()
                .UseStaticFiles()
                .UseConsulRegisterService(Configuration);

 
        }
    }
}
