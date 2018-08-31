//using IdentityServer4.Models;
//using NanoFabric.IdentityServer.Interfaces.Repositories;
//using StackExchange.Redis;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace NanoFabric.IdentityServer.Repositories.PersistedGrantAggregate
//{
//    public class PersistedGrantStore : IPersistedGrantRepository, IDisposable
//    {
//        private const string _dateFormatString = "yyyy-MM-dd HH:mm:ss";
//        private const int _dbNumber = 2;
//        private readonly ConnectionMultiplexer _redis;

//        public PersistedGrantStore(IdentityOptions options)
//        {
//            _redis = ConnectionMultiplexer.Connect(options.Redis);
//        }

//        public void Dispose()
//        {
//            _redis.Dispose();
//        }

//        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
//        {
//            var db = _redis.GetDatabase(_dbNumber);
//            var keys = await db.ListRangeAsync(subjectId);
//            var list = new List<PersistedGrant>();
//            foreach (string key in keys)
//            {
//                var items = await db.HashGetAllAsync(key);

//                list.Add(GetPersistedGrant(items));
//            }

//            return list;
//        }

//        public async Task<PersistedGrant> GetAsync(string key)
//        {
//            var db = _redis.GetDatabase(_dbNumber);
//            var items = await db.HashGetAllAsync(key);

//            return GetPersistedGrant(items);
//        }

//        public async Task RemoveAllAsync(string subjectId, string clientId)
//        {
//            var db = _redis.GetDatabase(_dbNumber);
//            await db.KeyDeleteAsync($"{subjectId}:{clientId}");
//        }

//        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
//        {
//            var db = _redis.GetDatabase(_dbNumber);
//            await db.KeyDeleteAsync($"{subjectId}:{clientId}:{type}");
//        }

//        public async Task RemoveAsync(string key)
//        {
//            var db = _redis.GetDatabase(_dbNumber);
//            await db.KeyDeleteAsync(key);
//        }

//        public async Task StoreAsync(PersistedGrant grant)
//        {
//            var db = _redis.GetDatabase(_dbNumber);

//            await db.HashSetAsync(grant.Key, GetHashEntries(grant));
//            await db.KeyExpireAsync(grant.Key, grant.Expiration);

//            await db.ListLeftPushAsync(grant.SubjectId, grant.Key);
//            await db.KeyExpireAsync(grant.SubjectId, grant.Expiration);

//            var key1 = $"{grant.SubjectId}:{grant.ClientId}";
//            await db.ListLeftPushAsync(key1, grant.Key);
//            await db.KeyExpireAsync(key1, grant.Expiration);

//            var key2 = $"{grant.SubjectId}:{grant.ClientId}:{grant.Type}";
//            await db.ListLeftPushAsync(key2, grant.Key);
//            await db.KeyExpireAsync(key2, grant.Expiration);
//        }

//        private HashEntry[] GetHashEntries(PersistedGrant grant)
//        {
//            return new HashEntry[]
//            {
//                new HashEntry("key", grant.Key),
//                new HashEntry("type", grant.Type),
//                new HashEntry("sub", grant.SubjectId),
//                new HashEntry("client", grant.ClientId),
//                new HashEntry("create", grant.CreationTime.ToString(_dateFormatString)),
//                new HashEntry("expire", grant.Expiration == null ? default(DateTime).ToString(_dateFormatString) : grant.Expiration.Value.ToString(_dateFormatString)),
//                new HashEntry("data", grant.Data),
//            };
//        }

//        private PersistedGrant GetPersistedGrant(HashEntry[] entries)
//        {
//            if (entries.Length != 7)
//                return null;

//            var grant = new PersistedGrant();
//            foreach (var item in entries)
//            {
//                if (item.Name == "key")
//                {
//                    grant.Key = item.Value;
//                }
//                if (item.Name == "type")
//                {
//                    grant.Type = item.Value;
//                }
//                if (item.Name == "sub")
//                {
//                    grant.SubjectId = item.Value;
//                }
//                if (item.Name == "client")
//                {
//                    grant.ClientId = item.Value;
//                }
//                if (item.Name == "create")
//                {
//                    grant.CreationTime = DateTime.Parse(item.Value);
//                }
//                if (item.Name == "expire")
//                {
//                    grant.Expiration = DateTime.Parse(item.Value);
//                    if (grant.Expiration.Value == default(DateTime))
//                    {
//                        grant.Expiration = null;
//                    }
//                }
//                if (item.Name == "data")
//                {
//                    grant.Data = item.Value;
//                }
//            }

//            return grant;
//        }
//    }
//}