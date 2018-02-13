using Butterfly.Client.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoFabric.AspNetCore;
using NanoFabric.Router;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System.Threading;

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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNanoFabricConsul(Configuration);
            services.AddNanoFabricConsulRouter();
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();
            services.AddOptions();
            var collectorUrl = Configuration.GetValue<string>("Butterfly:CollectorUrl");
            services.AddSwaggerGen(option => { option.SwaggerDoc("v1", new Info { Title = "SampleService.Kestrel http api", Version = "v1" }); });
            services.AddButterfly(option =>
            {
                option.CollectorUrl = collectorUrl;
                option.Service = "SampleService_Kestrel";
            });
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            var log = loggerFactory
                      .AddNLog()
                      .CreateLogger<Startup>();

            loggerFactory.ConfigureNLog("NLog.config");

           //var authority = Configuration.GetValue<string>("AppSetting:IdentityServerAuthority");

            //app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            //{
            //    Authority = authority,

            //    RequireHttpsMetadata = false,

            //    ApiName = "api1",

            //    ApiSecret = "myApiSecret"

            //});
            app.UseMvc(routes =>
             {
                 routes.MapRoute(
                     name: "default",
                     template: "{controller=Home}/{action=Index}/{id?}");
             });
            app.UseConsulRegisterService(Configuration);
        }
    }
}
