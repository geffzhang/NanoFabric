using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoFabric.IdentityServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace SampleService.IdentityServer
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;

        public Startup(IHostingEnvironment env)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var cert = new X509Certificate2(Path.Combine(_environment.ContentRootPath, "nanofabrictest.pfx"), "idsrv3test");

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddMvc()
               .AddJsonOptions(options =>
               {
                   options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                   options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                   options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                   options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

               })  ;

            services.AddIdentityServer()
                .AddSigningCredential(cert)
                .AddNanoFabricIDS(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", corsBuilder =>
                {
                    corsBuilder.AllowAnyHeader();
                    corsBuilder.AllowAnyMethod();
                    corsBuilder.AllowAnyOrigin();
                    corsBuilder.AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseCors("CorsPolicy");
            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
