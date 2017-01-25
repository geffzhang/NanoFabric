using CFT.NanoFabric.Domain.Models;
using System.Threading.Tasks;

namespace CFT.NanoFabric.Domain.Repositories
{
    public interface IAddableRepository<TDomainEntity, TKey> : IRepository<TDomainEntity, TKey>
       where TDomainEntity : DomainEntity<TKey>, IAggregateRoot
    {

        Task AddAsync(TDomainEntity entity);

    }
}