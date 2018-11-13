using Microsoft.Extensions.DependencyInjection;

namespace NanoFabric.Exceptionless
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNanoFabricExceptionless(this IServiceCollection services)
        {            
            services.AddSingleton<ILessLog, LessLog>();
            services.AddSingleton<ILessLinksLog, LessLinksLog>();
            services.AddSingleton<ILessFeatureLog, LessFeatureLog>();
            services.AddSingleton<ILessExceptionLog, LessExceptionLog>();

            return services;
        }
    }
}
