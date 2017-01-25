using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4Demo.Models
{
    public class MyClientStore : IClientStore
    {
        readonly Dictionary<string, Client> _clients;
        readonly IResourceStore _scopes;

        public MyClientStore(IResourceStore scopes)
        {
            _scopes = scopes;
            _clients = new Dictionary<string, Client>()
            {
                {
                    "client",
                    new Client
                    {
                       ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedScopes = { "api1" }
                    }
                },
                { "mvc",
                    new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RedirectUris = { "http://localhost:5001/signin-oidc" },
                    AllowedScopes = { "openid", "profile", "api1" }
                }
                }
            };
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            Client client;
            _clients.TryGetValue(clientId, out client);
            return Task.FromResult(client);
        }
    }
}
