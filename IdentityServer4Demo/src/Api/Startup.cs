using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddJsonFormatters()
                .AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Trace);
            app.UseDeveloperExceptionPage();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = "http://localhost:5000",
                RequireHttpsMetadata = false,

                 AllowedScopes = { "api1" }
            });

            app.UseMvc();
        }
    }
}