using Microsoft.AspNetCore.Mvc;

namespace WebAPIGateway
{
    [Route("/")]
    public class HomeController : Controller
    {
        public IActionResult Index() {
            return Json("Welcome to WebAPIGateway!");
        }
    }
}