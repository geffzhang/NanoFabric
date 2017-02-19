using System;

namespace NanoFabric.Domain.Models
{
    public abstract class DomainEntity<TKey>
    {
        private TKey uniqueId;

        public TKey Id
        {
            get
            {
                return uniqueId;
            }
        }

        protected DomainEntity(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                throw new ArgumentOutOfRangeException(nameof(id), "The identifier cannot be equal to the default value of the type.");
            }

            uniqueId = id;
        }

        public override bool Equals(object obj)
        {
            var entity = obj as DomainEntity<TKey>;

            if (entity == null)
            {
                return false;
            }
            else
            {
                return uniqueId.Equals(entity.Id);
            }
        }

        public static bool operator ==(DomainEntity<TKey> x, DomainEntity<TKey> y)
        {
            if ((object)x == null)
            {
                return (object)y == null;
            }

            if ((object)y == null)
            {
                return (object)x == null;
            }

            return x.Id.Equals(y.Id);
        }

        public static bool operator !=(DomainEntity<TKey> x, DomainEntity<TKey> y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return uniqueId.GetHashCode();
        }
    }
}