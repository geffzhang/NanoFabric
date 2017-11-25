using NanoFabric.RegistryHost.ConsulRegistry;
using NanoFabric.Router;
using Microsoft.Extensions.DependencyInjection;
using System;

class Program
{
    static void Main(string[] args)
    {
        ServiceCollection services = new ServiceCollection(); // 准备好我们的容器
        var registryClient = BuildRegistryClient("urlprefix-");
        var uri = new Uri("http://localhost:9030/values");
        var results = registryClient.FindServiceInstancesAsync(uri).Result;
        if (results != null)
        {
            var registerHost = registryClient.Choose(results);

            Console.WriteLine($"{registerHost.Address }:{registerHost.Port}");
        }
        Console.ReadLine();
    }

    private  static RegistryClient BuildRegistryClient(string prefixName)
    {
        var configuration = new ConsulRegistryHostConfiguration() { HostName = "localhost" };

        var consul = new ConsulRegistryHost(configuration);

        var registryClient = new RegistryClient(prefixName, new RoundRobinAddressRouter());
        registryClient.AddRegistryHost(consul);

        return registryClient;
    }



}