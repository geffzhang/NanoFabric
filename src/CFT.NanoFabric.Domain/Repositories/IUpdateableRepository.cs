using CFT.NanoFabric.Domain.Models;
using System.Threading.Tasks;

namespace CFT.NanoFabric.Domain.Repositories
{
    public interface IUpdateableRepository<TDomainEntity, TKey> : IRepository<TDomainEntity, TKey>
        where TDomainEntity : DomainEntity<TKey>, IAggregateRoot
    {
        Task UpdateAsync(TDomainEntity entity);
    }
}