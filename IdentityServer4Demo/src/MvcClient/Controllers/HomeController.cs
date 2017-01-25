using Microsoft.AspNetCore.Mvc;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}