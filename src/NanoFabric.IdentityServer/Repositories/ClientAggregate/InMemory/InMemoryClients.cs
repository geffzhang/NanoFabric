using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace NanoFabric.IdentityServer.Repositories.ClientAggregate.InMemory
{
    public class InMemoryClients
    {
        public static IEnumerable<Client> Clients = new List<Client>
        {
            new Client
            {
                    ClientId = "mvc.hybrid",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    RedirectUris = { "http://localhost:5001/signin-oidc" },
                    AllowedScopes = { "openid", "profile", "api1" }

            }
        };
    }
}
