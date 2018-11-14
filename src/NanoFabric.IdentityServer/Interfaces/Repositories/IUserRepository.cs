using NanoFabric.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.IdentityServer.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(string username);
        Task<User> GetAsync(string username, string password);
        Task AddAsync(User entity, string password);
        Task DeleteAsync(int id);
        Task UpdateAsync(User entity);
        Task<User> GetAsync(int id);
    }
}
