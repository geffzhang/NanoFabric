using NanoFabric.IdentityServer.Interfaces.Repositories;
using NanoFabric.IdentityServer.Interfaces.Services;
using NanoFabric.IdentityServer.Repositories.ClientAggregate.InMemory;
using NanoFabric.IdentityServer.Repositories.ResourceAggregate.InMemory;
using NanoFabric.IdentityServer.Repositories.UserAggregate.InMemory;
using NanoFabric.IdentityServer.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
 
namespace NanoFabric.IdentityServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNanoFabricIdentityServer(this IServiceCollection services)
        {
            //repositories
            services.AddTransient<IUserRepository, UserInMemoryRepository>();
            services.AddTransient<IResourceRepository, ResourceInMemoryRepository>();
            services.AddTransient<IClientRepository, ClientInMemoryRepository>();
            services.AddTransient<IClientStore, ClientInMemoryRepository>();
            services.AddTransient<IResourceStore, ResourceInMemoryRepository>();
            //services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPasswordService, PasswordService>();
            //validators
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

            return services;
        }
    }
}
