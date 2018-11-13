using Microsoft.Extensions.DependencyInjection;
using NanoFabric.Core.MqMessages;
using NanoFabric.MqMessages.RebusCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.MqMessages
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMqMessages(this IServiceCollection services 
           )
        {
            services.AddSingleton<IMqMessagePublisher, RebusRabbitMqPublisher>();
            return services;
        }
    }
}
