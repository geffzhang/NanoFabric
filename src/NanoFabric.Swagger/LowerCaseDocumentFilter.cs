using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace NanoFabric.Swagger
{
    /// <summary>
    /// Converts Swagger document paths to lower case.
    /// </summary>
    public sealed class LowerCaseDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths = swaggerDoc.Paths.ToDictionary(x => x.Key.ToLower(), x => x.Value);
        }
    }
}
