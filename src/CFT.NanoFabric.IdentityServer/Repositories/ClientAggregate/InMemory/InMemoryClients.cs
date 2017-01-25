using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace CFT.NanoFabric.IdentityServer.Repositories.ClientAggregate.InMemory
{
    public class InMemoryClients
    {
        public static IEnumerable<Client> Clients = new List<Client>
        {
            new Client
                    {
                       ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedScopes = { "api1" }
                    },
             new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    RedirectUris = { "http://localhost:5001/signin-oidc" },
                    AllowedScopes = { "openid", "profile", "api1" }
                }
           
        };
    }
}
