using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.AspNetCore.Cors
{
    /// <summary>
    /// 一劳永逸：域名支持通配符，ASP.NET Core中配置CORS
    /// http://www.cnblogs.com/dudu/p/5895424.html
    /// </summary>
    public class WildcardCorsService : CorsService
    {
        public WildcardCorsService(IOptions<CorsOptions> options)
            : base(options)
        {
        }

        public override void EvaluateRequest(HttpContext context, CorsPolicy policy, CorsResult result)
        {
            var origin = context.Request.Headers[CorsConstants.Origin];
            EvaluateOriginForWildcard(policy.Origins, origin);
            base.EvaluateRequest(context, policy, result);
        }

        public override void EvaluatePreflightRequest(HttpContext context, CorsPolicy policy, CorsResult result)
        {
            var origin = context.Request.Headers[CorsConstants.Origin];
            EvaluateOriginForWildcard(policy.Origins, origin);
            base.EvaluatePreflightRequest(context, policy, result);
        }

        private void EvaluateOriginForWildcard(IList<string> origins, string origin)
        {
            //...
        }
    }
}
