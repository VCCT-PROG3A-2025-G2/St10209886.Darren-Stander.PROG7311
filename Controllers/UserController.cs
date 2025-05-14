using Microsoft.AspNetCore.Mvc;
using TestApp.Models;
using TestApp.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TestApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationContext _context;

        public UserController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user, string ContactInfo, string Address)
        {
            // 🔥 Conditional Validation
            if (user.Role == "Farmer")
            {
                if (string.IsNullOrEmpty(ContactInfo))
                {
                    ModelState.AddModelError("ContactInfo", "Contact Info is required for Farmers.");
                }

                if (string.IsNullOrEmpty(Address))
                {
                    ModelState.AddModelError("Address", "Address is required for Farmers.");
                }
            }
            else
            {
                // 🔥 Remove potential errors if it's an Employee registration
                ModelState.Remove("ContactInfo");
                ModelState.Remove("Address");
            }

            if (ModelState.IsValid)
            {
                // 🔥 Save the User
                _context.Users.Add(user);
                _context.SaveChanges();

                // 🔥 If the role is "Farmer", also create a Farmer profile
                if (user.Role == "Farmer")
                {
                    var farmer = new Farmer
                    {
                        Name = user.Username,
                        ContactInfo = ContactInfo,
                        Address = Address,
                        UserId = user.Id
                    };

                    // Save the Farmer details
                    _context.Farmers.Add(farmer);
                    _context.SaveChanges();

                    // 🔥 Now we link the FarmerId to the User
                    user.FarmerId = farmer.Id;
                    _context.Users.Update(user);
                    _context.SaveChanges();

                    // Set the FarmerId in Session
                    HttpContext.Session.SetInt32("FarmerId", farmer.Id);

                    // 🔥 Redirect to Product page for Farmers
                    return RedirectToAction("Index", "Product");
                }

                // 🔥 If the role is Employee, just login and redirect
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                // 🔥 Redirect to Home for Employees
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        // GET: User/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            var loggedInUser = _context.Users
                .FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

            if (loggedInUser != null)
            {
                HttpContext.Session.SetString("Username", loggedInUser.Username);
                HttpContext.Session.SetString("Role", loggedInUser.Role);

                if (loggedInUser.FarmerId != null)
                {
                    HttpContext.Session.SetInt32("FarmerId", (int)loggedInUser.FarmerId);
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Invalid Credentials";
            return View();
        }

        // GET: User/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
