using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Stores;

namespace NanoFabric.IdentityServer.Interfaces.Repositories
{
    public interface IClientRepository : IClientStore
    {
        Task<IEnumerable<string>> GetAllAllowedCorsOriginsAsync();
    }
}
