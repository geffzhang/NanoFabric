using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Api
{
    [Authorize]
    [Route("test")]
    public class TestController : ControllerBase
    {
        public IActionResult Get()
        {
            var claims = from c in User.Claims
                         select new { c.Type, c.Value };

            return new JsonResult(claims);
        }
    }
}