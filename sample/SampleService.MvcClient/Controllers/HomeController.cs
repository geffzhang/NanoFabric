using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Butterfly.Client.Tracing;
using Microsoft.AspNetCore.Mvc;
using NanoFabric.Router;
using NanoFabric.Router.Consul;

namespace SampleService.MvcClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About([FromServices]IServiceSubscriberFactory subscriberFactory, [FromServices] HttpClient httpClient, [FromServices] IServiceTracer tracer)
        {
            var serviceSubscriber = subscriberFactory.CreateSubscriber("NanoFabric_Ocelot", ConsulSubscriberOptions.Default, new NanoFabric.Router.Throttle.ThrottleSubscriberOptions() { MaxUpdatesPeriod = TimeSpan.FromSeconds(30), MaxUpdatesPerPeriod = 20 });
            serviceSubscriber.StartSubscription().ConfigureAwait(false).GetAwaiter().GetResult();
            serviceSubscriber.EndpointsChanged += async (sender, eventArgs) =>
            {
                // Reset connection pool, do something with this info, etc
                var endpoints = await serviceSubscriber.Endpoints();
                var servicesInfo = string.Join(",", endpoints);
            };
            ILoadBalancer loadBalancer = new RoundRobinLoadBalancer(serviceSubscriber);
            var endPoint = loadBalancer.Endpoint().ConfigureAwait(false).GetAwaiter().GetResult();

            string content = "Your application description page";
            if (endPoint != null)
            {
                content = httpClient.GetStringAsync($"{endPoint.ToUri()}api/values").Result;
                ViewData["Message"] = $"Get {endPoint.ToUri()}api/values: {content}.";
            }
            else
            {
                ViewData["Message"] = $"{content}.";
            }

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
