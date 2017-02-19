using NanoFabric.Domain.Models;
using System.Threading.Tasks;

namespace NanoFabric.Domain.Repositories
{
    public interface IRepository<TDomainEntity, TKey>
        where TDomainEntity : DomainEntity<TKey>, IAggregateRoot
    {
        Task<TDomainEntity> GetAsync(TKey id);
    }
}