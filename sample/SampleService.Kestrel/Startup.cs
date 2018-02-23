using Butterfly.Client.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoFabric.AspNetCore;
using NanoFabric.AspNetCore.Cors;
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
        /// https://damienbod.com/2018/02/02/securing-an-asp-net-core-mvc-application-which-uses-a-secure-api/
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNanoFabricConsul(Configuration);
            services.AddNanoFabricConsulRouter();

            services.AddAuthorization();
            services.AddCors();
            services.AddDistributedMemoryCache();

            var authority = Configuration.GetValue<string>("Authority");
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = authority;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";
                    options.ApiSecret = "secret";                    
                });
            services.AddAuthorization(options =>
                   options.AddPolicy("protectedScope", policy =>
                   {
                       policy.RequireClaim("scope", "scope_used_for_api_in_protected_zone");
                   })
               );

            services.AddMvc()
                .AddMvcApiResult();
            services.Add(ServiceDescriptor.Transient<ICorsService, WildcardCorsService>());
            services.Configure<CorsOptions>(options => options.AddPolicy(
                "AllowSameDomain",builder => builder.WithOrigins("*.*")));

            services.AddOptions();
            var collectorUrl = Configuration.GetValue<string>("Butterfly:CollectorUrl");
            //http://www.cnblogs.com/morang/p/8325729.html
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Version = "v1",
                        Title = "SampleService 接口文档",
                        Description = "SampleService 接口集成 Swashbuckle",
                        TermsOfService = "Values 和 Status（健康检查）.",

                        //Contact = new Contact { Name = "geffzhang", Email = "", Url = "http://github.com/geffzhang" },
                        //License = new License { Name = "Use under MIT", Url = "https://github.com/geffzhang/NanoFabric/blob/develop/LICENSE.md" }
                    }
                         
                );
                c.OperationFilter<AssignOperationVendorExtensions>();
                c.DocumentFilter<ApplyTagDescriptions>();
                
            });

            services.AddButterfly(option =>
            {
                option.CollectorUrl = collectorUrl;
                option.Service = "SampleService_Kestrel";
                option.IgnoredRoutesRegexPatterns = new string[] { "/status" };
            });
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            var log = loggerFactory
                      .AddNLog()
                      .CreateLogger<Startup>();

            loggerFactory.ConfigureNLog("NLog.config");
            env.EnvironmentName = EnvironmentName.Development;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.UseCors(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithExposedHeaders("WWW-Authenticate");
            });

            app.UseAuthentication();
            app.UseMvc();
            app.UseStaticFiles();
            app.UseSwagger(c =>
             {
                 c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
             });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                //注入汉化文件
                c.InjectOnCompleteJavaScript($"/lib/swagger/swagger_translator.js");
            });
            app.UseConsulRegisterService(Configuration);
        }
    }
}
