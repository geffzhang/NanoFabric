using IdentityServer4;
using IdentityServer4.Stores;
using IdentityServer4Demo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Security.Cryptography;

namespace IdentityServer4Demo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                 .AddInMemoryCollection()   //将配置文件存储到内存中，方便运行时获取
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                        
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            //RSA：证书长度2048以上，否则抛异常
            //配置AccessToken的加密证书
            var rsa = new RSACryptoServiceProvider();
            //从配置文件获取加密证书
            rsa.ImportCspBlob(Convert.FromBase64String(Configuration["SigningCredential"]));

            services.AddMvc();
            services.AddSingleton<IClientStore, MyClientStore>();
            services.AddSingleton<IResourceStore, MyScopeStore>();
            services.AddIdentityServer()
                //.AddInMemoryScopes(Config.GetScope())
                //.AddInMemoryClients(Config.GetClients())
                //如果是client credentials模式那么就不需要设置验证User了
                //.AddTestUsers(Config.GetUsers())
                .AddResourceOwnerValidator<FITUserValidator>() //User验证接口
                .AddSigningCredential(new RsaSecurityKey(rsa)); //设置加密证书;
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            var serilog = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.LiterateConsole(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message}{NewLine}{Exception}{NewLine}")
                .CreateLogger();

            loggerFactory.AddSerilog(serilog);
            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();

            // cookie middleware for temporarily storing the outcome of the external authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });
            
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}