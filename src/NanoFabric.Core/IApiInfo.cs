using System.Collections.Generic;
using System.Reflection;

namespace NanoFabric.Core
{
    public interface IApiInfo
    {
        string AuthenticationAuthority { get; }

        string ApiName { get; }

        string ApiSecret { get; }

        string Title { get; }

        string Version { get; }

        Assembly ApplicationAssembly { get; }

        IDictionary<string, string> Scopes { get; }

        SwaggerAuthInfo SwaggerAuthInfo { get; }
        
    }

    public class SwaggerAuthInfo
    {
        public string ClientId { get; }
        public string Secret { get; }
        public string Realm { get;  }

        public SwaggerAuthInfo(
            string clientId, 
            string secret, 
            string realm
            )
        {
            ClientId = clientId;
            Secret = secret;
            Realm = realm;
        }
    }
}
