using CFT.NanoFabric.IdentityServer.Interfaces.Repositories;
using CFT.NanoFabric.IdentityServer.Interfaces.Services;
using CFT.NanoFabric.IdentityServer.Repositories.ClientAggregate.InMemory;
using CFT.NanoFabric.IdentityServer.Repositories.ResourceAggregate.InMemory;
using CFT.NanoFabric.IdentityServer.Repositories.UserAggregate.InMemory;
using CFT.NanoFabric.IdentityServer.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// https://github.com/TheFeedBackNetwork/TheFeedbackNetworkApi
/// </summary>
namespace CFT.NanoFabric.IdentityServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNanoFabric(this IServiceCollection services)
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
