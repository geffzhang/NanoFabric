using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NanoFabric.Core;
using System.Linq;
using NanoFabric.AspNetCore.Helper;

namespace NanoFabric.AspNetCore.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _whiteListIps;
        private readonly ILogger _logger;

        public AuthenticationMiddleware(RequestDelegate next, string whiteListIps, ILoggerFactory loggerFactory)
        {
            _next = next;
            _whiteListIps = whiteListIps;
            _logger = loggerFactory.CreateLogger<AuthenticationMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (ValidateIfIpIsInWhiteList(context))
            {
                await _next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
        }

        private bool ValidateIfIpIsInWhiteList(HttpContext context)
        {
            var remoteIp = IPAddressHelper.GetRequestIP(context);

            _logger.LogInformation($"访问 IP地址为 RemoteIP:{remoteIp}");

            string[] allowedIps = _whiteListIps.Split(';');
            if (!allowedIps.Any(ip => ip == remoteIp.ToString()))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return false;
            }
            return true;
        }     

    }

    public static class AuthenticationMiddlewareExtension
    {
        /// <summary>
        /// Habilita o uso do Middleware de autenticação básica
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder app, string whiteListIps, string path="/api" )
        {
            return app.UseWhen(x => (x.Request.Path.StartsWithSegments(path, StringComparison.OrdinalIgnoreCase)),
                builder =>
                {
                    builder.UseMiddleware<AuthenticationMiddleware>(whiteListIps);
                });
        }
    }
}