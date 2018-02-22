using NanoFabric.WebApi.ApiWidgets;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IMvcCoreBuilderApiResultExtensions
    {
        public static IMvcBuilder AddMvcApiResult(this IMvcBuilder builder)
        {
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.AddMvcOptions(options => {
                options.Filters.Add(typeof(ApiResultFilterAttribute));
                options.Filters.Add(typeof(ApiExceptionFilterAttribute));
            });
        }

        public static IMvcCoreBuilder AddMvcApiResult(this IMvcCoreBuilder builder)
        {
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.AddMvcOptions(options => {
                options.Filters.Add(typeof(ApiResultFilterAttribute));
                options.Filters.Add(typeof(ApiExceptionFilterAttribute));
            });
        }
    }
}
