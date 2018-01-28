using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NanoFabric.Router;
using System;
using System.Net.Http;

class Program
{
    static void Main(string[] args)
    {
        //string consulHost = "localhost";
        //int consulPort =  8500;

        //var consul = new ConsulClient(config =>
        //{
        //    config.Address = new Uri($"http://{consulHost}:{consulPort}");
        //});

        //ServiceCollection services = new ServiceCollection(); // 准备好我们的容器

        //services.AddSingleton<IConsulClient>(consul);
        //services.AddCacheServiceSubscriber();
        //services.AddConsulServiceDiscovery();
        //services.TryAddTransient<IServiceSubscriberFactory, ServiceSubscriberFactory>();
        //IServiceSubscriberFactory subscriberFactory = services.BuildServiceProvider().GetRequiredService<IServiceSubscriberFactory>();
        //// 创建ConsoleLogProvider并根据日志类目名称（CategoryName）生成Logger实例
        //var logger = services.AddLogging().BuildServiceProvider().GetService<ILoggerFactory>().AddConsole().CreateLogger("App");

        //var serviceSubscriber = subscriberFactory.CreateSubscriber("FooService");
        //serviceSubscriber.StartSubscription().ConfigureAwait(false).GetAwaiter().GetResult();
        //serviceSubscriber.EndpointsChanged += async (sender, eventArgs) =>
        //{
        //    // Reset connection pool, do something with this info, etc
        //    var endpoints = await serviceSubscriber.Endpoints();
        //    var servicesInfo = string.Join(",", endpoints);
        //    logger.LogInformation($"Received updated subscribers [{servicesInfo}]");
        //};
        var httpClient = new HttpClient();
        var traceid = Guid.NewGuid().ToString();
        httpClient.DefaultRequestHeaders.Add("ot-traceid", traceid);
         httpClient.GetStringAsync("http://localhost:5000/api/values");
        Console.WriteLine(traceid);
        System.Console.ReadLine();
    }
}