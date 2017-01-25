using CFT.NanoFabric.IdentityServer.Interfaces.Repositories;
using CFT.NanoFabric.IdentityServer.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CFT.NanoFabric.IdentityServer.Interfaces.Services
{
    public interface IUserService : IUserRepository
    {        
        Task<User> AutoProvisionUserAsync(string provider, string userId, List<Claim> claims);
        Task<bool> ValidateCredentialsAsync(string username, string password);
        Task<User> FindByExternalProviderAsync(string provider, string userId);
    }
}
