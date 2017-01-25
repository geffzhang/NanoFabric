using CFT.NanoFabric.Domain.Models;
using System;
using System.Threading.Tasks;

namespace CFT.NanoFabric.Domain.Repositories
{
    public interface IDeleteableRepository<TDomainEntity, TKey> : IRepository<TDomainEntity, TKey>
        where TDomainEntity : DomainEntity<TKey>, IAggregateRoot
    {
        Task DeleteAsync(TKey id);
    }
}