using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer4Demo
{
    public class Config
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
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
}
