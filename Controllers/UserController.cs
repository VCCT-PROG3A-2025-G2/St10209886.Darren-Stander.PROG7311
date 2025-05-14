// Start of file
using Microsoft.AspNetCore.Mvc;
using TestApp.Models;
using TestApp.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TestApp.Controllers
{
    // Start of UserController class
    public class UserController : Controller
    {
        private readonly ApplicationContext _context;

        public UserController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: /User/Register
        // Shows the registration form for both Farmers and Employees.
        public IActionResult Register()
        {
            return View();
        }

        //  /User/Register
        // Processes registration:
        // - For Farmers, validates ContactInfo and Address, creates Farmer profile, links to User, sets session.
        // - For Employees, skips Farmer fields and logs them in directly.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user, string ContactInfo, string Address)
        {
            // Validate farmer-specific fields
            if (user.Role == "Farmer")
            {
                if (string.IsNullOrEmpty(ContactInfo))
                    ModelState.AddModelError("ContactInfo", "Contact Info is required for Farmers.");

                if (string.IsNullOrEmpty(Address))
                    ModelState.AddModelError("Address", "Address is required for Farmers.");
            }
            else
            {
                // Remove validation for Employee registrations
                ModelState.Remove("ContactInfo");
                ModelState.Remove("Address");
            }

            if (ModelState.IsValid)
            {
                // Save User record
                _context.Users.Add(user);
                _context.SaveChanges();

                if (user.Role == "Farmer")
                {
                    // Create and save Farmer profile linked to the user
                    var farmer = new Farmer
                    {
                        Name = user.Username,
                        ContactInfo = ContactInfo,
                        Address = Address,
                        UserId = user.Id
                    };
                    _context.Farmers.Add(farmer);
                    _context.SaveChanges();

                    // Update User with FarmerId
                    user.FarmerId = farmer.Id;
                    _context.Users.Update(user);
                    _context.SaveChanges();

                    // Store FarmerId in session and redirect to Products
                    HttpContext.Session.SetInt32("FarmerId", farmer.Id);
                    return RedirectToAction("Index", "Product");
                }

                // Employee login: set session and redirect home
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);
                return RedirectToAction("Index", "Home");
            }

            // If validation fails, re-display form with errors
            return View(user);
        }

        // GET: /User/Login
        // Displays the login form.
        public IActionResult Login()
        {
            return View();
        }

        //  /User/Login
        // Authenticates user credentials, sets session, and redirects.
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

                if (loggedInUser.FarmerId.HasValue)
                    HttpContext.Session.SetInt32("FarmerId", loggedInUser.FarmerId.Value);

                return RedirectToAction("Index", "Home");
            }

            // Invalid credentials: show error
            ViewBag.Message = "Invalid Credentials";
            return View();
        }

        // GET: /User/Logout
        // Clears session and redirects to home page.
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
    // End of UserController class
}
 // End of file
