using NanoFabric.IdentityServer.Interfaces.Repositories;
using NanoFabric.IdentityServer.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NanoFabric.IdentityServer.Interfaces.Services
{
    public interface IUserService : IUserRepository
    {        
        Task<User> AutoProvisionUserAsync(string provider, string userId, List<Claim> claims);
        Task<bool> ValidateCredentialsAsync(string username, string password);
        Task<User> FindByExternalProviderAsync(string provider, string userId);
    }
}
