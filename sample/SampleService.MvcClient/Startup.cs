using Butterfly.Client.AspNetCore;
using Butterfly.Client.Tracing;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NanoFabric.AspNetCore;
using NanoFabric.Router;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;

namespace SampleService.MvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddNanoFabricConsul(Configuration);
            services.AddNanoFabricConsulRouter();

            var collectorUrl = Configuration.GetValue<string>("Butterfly:CollectorUrl");
            services.AddButterfly(option =>
            {
                option.CollectorUrl = collectorUrl;
                option.Service = "SampleService_MvcClient";
                option.IgnoredRoutesRegexPatterns = new string[] { "/status" };
            });

            services.AddSingleton<HttpClient>(p => new HttpClient(p.GetService<HttpTracingHandler>()));
            var authority = Configuration.GetValue<string>("Authority");
            services.AddAuthentication(options =>
            {
                options.DefaultScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = "oidc";
            })
           .AddCookie(options =>
           {
               options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
               options.Cookie.Name = "mvchybrid";
           })
           .AddOpenIdConnect("oidc", options =>
           {
               options.Authority = authority;
               options.RequireHttpsMetadata = false;

               options.ClientSecret = "secret";
               options.ClientId = "mvc.hybrid";

               options.ResponseType = "code id_token";

               options.Scope.Clear();
               options.Scope.Add("openid");
               options.Scope.Add("profile");
               options.Scope.Add("email");
               options.Scope.Add("api1");
               options.Scope.Add("idbase");
               options.Scope.Add("offline_access");

               options.ClaimActions.Remove("amr");
               options.ClaimActions.MapJsonKey("website", "website");

               options.GetClaimsFromUserInfoEndpoint = true;
               options.SaveTokens = true;

                // Map here the claims for name and role 
                options.TokenValidationParameters =
                   new TokenValidationParameters
                   {
                       NameClaimType = JwtClaimTypes.Email,
                       RoleClaimType = JwtClaimTypes.Role,
                   };

               options.Events.OnRedirectToIdentityProvider = context =>
               {
                   context.ProtocolMessage.SetParameter("culture",
                       CultureInfo.CurrentUICulture.Name);

                   return Task.FromResult(0);
               };

               options.Events.OnTicketReceived = async context =>
               {

               };
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

            app.UseStaticFiles();
            app.UseConsulRegisterService(Configuration);
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();            
        }
    }
}
