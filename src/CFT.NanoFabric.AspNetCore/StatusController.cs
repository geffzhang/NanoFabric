
using Microsoft.AspNetCore.Mvc;

namespace CFT.NanoFabric.WebApi
{
    public class StatusController : ApiControllerBase
    {
        [Route("/status")]
        public string GetStatus()
        {
            return "OK";
        }
    }
}
