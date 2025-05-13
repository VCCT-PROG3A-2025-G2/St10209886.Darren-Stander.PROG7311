using Microsoft.AspNetCore.Mvc;
using TestApp.Models;
using TestApp.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestApp.Controllers
{
    [AuthorizeEmployee] // Restricts access to Employees only
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
            return View(farmer);
        }

        public IActionResult Details(int id)
        {
            var farmer = _context.Farmers.FirstOrDefault(f => f.Id == id);
            if (farmer == null)
            {
                return NotFound();
            }
            return View(farmer);
        }
    }

    // Custom Authorization Attribute for Employee
    public class AuthorizeEmployeeAttribute : TypeFilterAttribute
    {
        public AuthorizeEmployeeAttribute() : base(typeof(EmployeeAuthorizationFilter)) { }
    }

    public class EmployeeAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");
            if (role != "Employee")
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
            }
        }
    }
}
