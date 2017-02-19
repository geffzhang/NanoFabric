using NanoFabric.Domain.Repositories;
using NanoFabric.IdentityServer.Models;
using System;
using System.Threading.Tasks;

namespace NanoFabric.IdentityServer.Interfaces.Repositories
{
    public interface IUserRepository : IAddableRepository<User, int>, IDeleteableRepository<User, int>, IUpdateableRepository<User,int>
    {
        Task<User> GetAsync(string username);
        Task<User> GetAsync(string username,string password);
        Task AddAsync(User entity, string password);
    }
}
