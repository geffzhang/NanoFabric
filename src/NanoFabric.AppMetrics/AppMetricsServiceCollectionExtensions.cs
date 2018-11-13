using App.Metrics;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NanoFabric.AppMetrics
{
    public static class AppMetricsServiceCollectionExtensions
    {
        public static IServiceCollection AddAppMetrics(this IServiceCollection services,
            Action<AppMetricsOptions> action)
        {
            var appMetricsConfig = new AppMetricsOptions();

            action(appMetricsConfig);

            var uri = new Uri(appMetricsConfig.ConnectionString);

            var metrics = App.Metrics.AppMetrics.CreateDefaultBuilder()
                .Configuration.Configure(
                    options =>
                    {
                        if (!string.IsNullOrWhiteSpace(appMetricsConfig.App))
                            options.AddAppTag(appMetricsConfig.App);

                        if (!string.IsNullOrWhiteSpace(appMetricsConfig.Env))
                            options.AddEnvTag(appMetricsConfig.Env);
                    })
                .Report.ToInfluxDb(
                    options =>
                    {
                        options.InfluxDb.BaseUri = uri;
                        options.InfluxDb.Database = appMetricsConfig.DataBaseName;
                        options.InfluxDb.UserName = appMetricsConfig.UserName;
                        options.InfluxDb.Password = appMetricsConfig.Password;
                        options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                        options.HttpPolicy.FailuresBeforeBackoff = 5;
                        options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                        options.FlushInterval = TimeSpan.FromSeconds(5);
                    })
                .Build();

            services.AddMetrics(metrics);
            services.AddMetricsReportScheduler();
            services.AddMetricsTrackingMiddleware();
            services.AddMetricsEndpoints();

            return services;
        }
    }
}