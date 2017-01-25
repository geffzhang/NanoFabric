using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace MvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Information);

            loggerFactory.AddDebug();
            app.UseDeveloperExceptionPage();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies"
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",

                Authority = "http://localhost:5000",
                RequireHttpsMetadata = false,

                ClientId = "mvc",
                ClientSecret = "secret",

                ResponseType = "code id_token",
                Scope = { "openid", "profile", "api1" },
                
                SaveTokens = true
            });

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}