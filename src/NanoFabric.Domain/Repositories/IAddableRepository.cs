using NanoFabric.Domain.Models;
using System.Threading.Tasks;

namespace NanoFabric.Domain.Repositories
{
    public interface IAddableRepository<TDomainEntity, TKey> : IRepository<TDomainEntity, TKey>
       where TDomainEntity : DomainEntity<TKey>, IAggregateRoot
    {

        Task AddAsync(TDomainEntity entity);

    }
}