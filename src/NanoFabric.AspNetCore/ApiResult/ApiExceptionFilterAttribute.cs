using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NanoFabric.Core.Exceptions;
using Newtonsoft.Json;

namespace NanoFabric.WebApi.ApiWidgets
{
    /// <summary>
    /// Represents a filter to handle Api exception.
    /// </summary>
    internal class ApiExceptionFilterAttribute : ExceptionFilterAttribute {
        private readonly ILogger<ApiExceptionFilterAttribute> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiExceptionFilterAttribute" /> class.
        /// </summary>
        /// <param name="logger">The logger</param>
        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger) {
            _logger = logger;
        }

        /// <summary>
        /// Called when [exception].
        /// </summary>
        /// <param name="context">The context.</param>
        public override void OnException(ExceptionContext context) {
            if (context.Exception is StandardException)
            {
                var exception = context.Exception as StandardException;

                context.Result = new ObjectResult(ApiResult.Failed(exception.InnerError, exception.InnerError.Message));

                // if is local request, will ip=, path=123, error= JSON, or...
                _logger.LogWarning($"ip={context.HttpContext.Connection.RemoteIpAddress}, path={context.HttpContext.Request.Path}, error={JsonConvert.SerializeObject(exception.InnerError)}");
            }
            else
            {
                _logger.LogError(0, context.Exception, $"ip={context.HttpContext.Connection.RemoteIpAddress}, path={context.HttpContext.Request.Path}, error={JsonConvert.SerializeObject(context.Exception.Message)}");

                context.Result = new ObjectResult(ApiResult.Failed(context.Exception.Message));
            }

            context.ExceptionHandled = true;
        }
    }
}
