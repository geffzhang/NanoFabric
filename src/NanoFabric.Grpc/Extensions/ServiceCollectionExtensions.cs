using Microsoft.Extensions.DependencyInjection;
using NanoFabric.Core;
using NanoFabric.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Grpc.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcClient(this IServiceCollection services)
        {
            services.AddSingleton<IGRpcConnection, GRpcConnection>();
            services.AddSingleton<IGrpcChannelFactory, GrpcChannelFactory>();
            return services;
        }
    }
}
