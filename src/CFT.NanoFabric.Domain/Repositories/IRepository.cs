using CFT.NanoFabric.Domain.Models;
using System.Threading.Tasks;

namespace CFT.NanoFabric.Domain.Repositories
{
    public interface IRepository<TDomainEntity, TKey>
        where TDomainEntity : DomainEntity<TKey>, IAggregateRoot
    {
        Task<TDomainEntity> GetAsync(TKey id);
    }
}