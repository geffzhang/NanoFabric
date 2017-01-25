using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer4Demo.Models
{
    public class MyScopeStore : IResourceStore
    {
        readonly static Dictionary<string, Scope> _scopes = new Dictionary<string, Scope>()
        {
            {
                "api1",
                new Scope
                {
                    Name = "api1",
                    DisplayName = "api1",
                    Description = "My API",
                }
            }
        };

        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            throw new NotImplementedException();
        }

        //public Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        //{
        //    List<Scope> scopes = new List<Scope>();
        //    if (scopeNames != null)
        //    {
        //        Scope sc;
        //        foreach (var sname in scopeNames)
        //        {
        //            if (_scopes.TryGetValue(sname, out sc))
        //            {
        //                scopes.Add(sc);
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    //返回值scopes不能为null
        //    return Task.FromResult<IEnumerable<Scope>>(scopes);
        //}

        public Task<Resources> GetAllResources()
        {
            throw new NotImplementedException();
        }

        //public Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        //{
        //    //publicOnly为true：获取public的scope；为false：获取所有的scope
        //    //这里不做区分
        //    return Task.FromResult<IEnumerable<Scope>>(_scopes.Values);
        //}
    }
}
