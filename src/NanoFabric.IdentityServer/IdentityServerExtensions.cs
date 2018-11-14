using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NanoFabric.IdentityServer.Interfaces.Repositories;
using NanoFabric.IdentityServer.Interfaces.Services;
using NanoFabric.IdentityServer.Repositories.ClientAggregate.InMemory;
using NanoFabric.IdentityServer.Repositories.ResourceAggregate.InMemory;
using NanoFabric.IdentityServer.Repositories.UserAggregate.InMemory;
using NanoFabric.IdentityServer.Services;
using StackExchange.Redis;

namespace NanoFabric.IdentityServer
{
    public static class IdentityServerExtensions
    {
        public static IIdentityServerBuilder AddNanoFabricIDS(this IIdentityServerBuilder builder, IConfigurationRoot config)
        {
            var option = builder.Services.ConfigurePOCO(config.GetSection("IdentityOptions"), () => new IdentityOptions());
            builder.Services.AddTransient<IUserRepository, UserInMemoryRepository>();
            builder.Services.AddTransient<IResourceRepository, ResourceInMemoryRepository>();
            builder.Services.AddTransient<IClientRepository, ClientInMemoryRepository>();
            builder.Services.AddTransient<IClientStore, ClientInMemoryRepository>();
            builder.Services.AddTransient<IResourceStore, ResourceInMemoryRepository>();

            builder.AddOperationalStore(options =>
             {
                 options.RedisConnectionString = option.Redis;
                 options.KeyPrefix = "ids_prefix";
             })
             .AddRedisCaching(options =>
             {
                 options.RedisConnectionString = option.Redis;
             });
            //services
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IPasswordService, PasswordService>();
            //validators
            builder.Services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

            builder.AddProfileService<ProfileService>();

            return builder;
        }

        public static IIdentityServerBuilder AddNanoFabricIdentityIDS(this IIdentityServerBuilder builder, IConfigurationRoot config)
        {
            var option = builder.Services.ConfigurePOCO(config.GetSection("IdentityOptions"), () => new IdentityOptions());
 
            builder.AddOperationalStore(options =>
            {
                options.RedisConnectionString = option.Redis;
                options.KeyPrefix = "ids_prefix";
            })
             .AddRedisCaching(options =>
             {
                 options.RedisConnectionString = option.Redis;
             });
 
            builder.AddProfileService<ProfileService>();

            return builder;
        }
    }
}
