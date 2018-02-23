using System.Collections.Generic;
using System.Linq;
using NanoFabric.Core;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NanoFabric.Swagger
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private readonly IApiInfo _apiInfo;

        public AuthorizeCheckOperationFilter(IApiInfo apiInfo)
        {
            _apiInfo = apiInfo;
        }

        public void Apply(
            Operation operation,
            OperationFilterContext context
        )
        {
            if (!context.HasAuthorize()) return;

            operation.Responses.Add("401", new Response { Description = "Unauthorized" });
            operation.Responses.Add("403", new Response { Description = "Forbidden" });

            operation.Security = new List<IDictionary<string, IEnumerable<string>>>
            {
                new Dictionary<string, IEnumerable<string>>
                {
                    {"oauth2", _apiInfo.Scopes.Keys.ToArray() }
                }
            };
        }
    }
}
