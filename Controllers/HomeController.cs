// Start of file
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TestApp.Controllers
{
    // Start of HomeController class
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Check if a user is logged in (session contains Username)
            if (HttpContext.Session.GetString("Username") != null)
            {
                // Pass username to the view and show logged-in layout
                ViewBag.Username = HttpContext.Session.GetString("Username");
                return View("LoggedIn");
            }
            
            return View();
        }

        public IActionResult AccessDenied()
        {
            // Display access denied page
            return View();
        }
    }
    // End of HomeController class
}
// End of file
