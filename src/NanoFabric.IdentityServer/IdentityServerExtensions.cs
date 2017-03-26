using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NanoFabric.IdentityServer.Interfaces.Repositories;
using NanoFabric.IdentityServer.Interfaces.Services;
using NanoFabric.IdentityServer.Repositories.ClientAggregate.InMemory;
using NanoFabric.IdentityServer.Repositories.PersistedGrantAggregate;
using NanoFabric.IdentityServer.Repositories.ResourceAggregate.InMemory;
using NanoFabric.IdentityServer.Repositories.UserAggregate.InMemory;
using NanoFabric.IdentityServer.Services;


namespace NanoFabric.IdentityServer
{
    public static class IdentityServerExtensions
    {
        public static IIdentityServerBuilder AddNanoFabricIDS(this IIdentityServerBuilder builder, IConfigurationRoot config)
        {
            builder.Services.ConfigurePOCO(config.GetSection("IdentityOptions"), () => new IdentityOptions());
            builder.Services.AddTransient<IUserRepository, UserInMemoryRepository>();
            builder.Services.AddTransient<IResourceRepository, ResourceInMemoryRepository>();
            builder.Services.AddTransient<IClientRepository, ClientInMemoryRepository>();
            builder.Services.AddTransient<IClientStore, ClientInMemoryRepository>();
            builder.Services.AddTransient<IResourceStore, ResourceInMemoryRepository>();
            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            //services
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IPasswordService, PasswordService>();
            //validators
            builder.Services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

            builder.AddProfileService<ProfileService>();

            return builder;
        }
    }
}
