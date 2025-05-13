using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TestApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                return View("LoggedIn");
            }
            return View();

        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
