using NanoFabric.Domain.Models;
using System;
using System.Threading.Tasks;

namespace NanoFabric.Domain.Repositories
{
    public interface IDeleteableRepository<TDomainEntity, TKey> : IRepository<TDomainEntity, TKey>
        where TDomainEntity : DomainEntity<TKey>, IAggregateRoot
    {
        Task DeleteAsync(TKey id);
    }
}