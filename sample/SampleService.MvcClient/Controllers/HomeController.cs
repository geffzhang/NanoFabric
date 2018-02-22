using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Butterfly.Client.Tracing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NanoFabric.Router;
using NanoFabric.Router.Consul;
using Newtonsoft.Json.Linq;

namespace SampleService.MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private IServiceSubscriberFactory subscriberFactory;
        private HttpClient httpClient;

        public HomeController(IServiceSubscriberFactory subscriberFactory, HttpClient httpClient)
        {
            this.subscriberFactory = subscriberFactory;
            this.httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About(/*[FromServices]IServiceSubscriberFactory subscriberFactory, [FromServices] HttpClient httpClient, [FromServices] IServiceTracer tracer*/)
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

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            var serviceSubscriber = subscriberFactory.CreateSubscriber("SampleService_Kestrel", ConsulSubscriberOptions.Default, new NanoFabric.Router.Throttle.ThrottleSubscriberOptions() { MaxUpdatesPeriod = TimeSpan.FromSeconds(30), MaxUpdatesPerPeriod = 20 });
            serviceSubscriber.StartSubscription().ConfigureAwait(false).GetAwaiter().GetResult();
            ILoadBalancer loadBalancer = new RoundRobinLoadBalancer(serviceSubscriber);
            var endPoint = loadBalancer.Endpoint().ConfigureAwait(false).GetAwaiter().GetResult();
            string token = await HttpContext.GetTokenAsync("access_token");
            httpClient.SetBearerToken(token);

            string response = await httpClient.GetStringAsync($"{endPoint.ToUri()}api/values/{new Random().Next()}");

            ViewBag.Json = JArray.Parse(response).ToString();

            return View();
        }
    }
}
