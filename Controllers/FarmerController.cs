// Start of file
using Microsoft.AspNetCore.Mvc;
using TestApp.Models;
using TestApp.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestApp.Controllers
{
    // Start of FarmerController class
    [AuthorizeEmployee]
    public class FarmerController : Controller
    {
        private readonly ApplicationContext _context;

        public FarmerController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var farmers = _context.Farmers.ToList();
            return View(farmers);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                _context.Farmers.Add(farmer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            // If validation fails, redisplay form with validation messages
            return View(farmer);
        }

        public IActionResult Details(int id)
        {
            var farmer = _context.Farmers.FirstOrDefault(f => f.Id == id);
            if (farmer == null)
            {
                // Return HTTP 404 if not found
                return NotFound();
            }
            return View(farmer);
        }
    }
    // End of FarmerController class

    // Start of AuthorizeEmployeeAttribute class
    public class AuthorizeEmployeeAttribute : TypeFilterAttribute
    {
        public AuthorizeEmployeeAttribute()
            : base(typeof(EmployeeAuthorizationFilter))
        { }
    }
    // End of AuthorizeEmployeeAttribute class

    // Start of EmployeeAuthorizationFilter class
    public class EmployeeAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");
            if (role != "Employee")
            {
                // Redirect non-employees to an AccessDenied page
                context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
            }
        }
    }
    // End of EmployeeAuthorizationFilter class
}
// End of file
