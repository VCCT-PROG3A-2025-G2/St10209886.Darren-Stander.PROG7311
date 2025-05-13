using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApp.Models;
using TestApp.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TestApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationContext _context;

        public ProductController(ApplicationContext context)
        {
            _context = context;
        }

        // 🔥 GET: Product/Index
        public IActionResult Index()
        {
            try
            {
                // 🔥 Get the Role and FarmerId from Session
                var role = HttpContext.Session.GetString("Role");
                var farmerId = HttpContext.Session.GetInt32("FarmerId");

                List<Product> products;

                // 🔥 If the user is a Farmer, only show their products
                if (role == "Farmer" && farmerId.HasValue)
                {
                    products = _context.Products
                                       .Include(p => p.Farmer)
                                       .Where(p => p.FarmerId == farmerId.Value)
                                       .ToList();
                }
                else
                {
                    // 🔥 If the user is an Employee, show all products
                    products = _context.Products.Include(p => p.Farmer).ToList();
                }

                return View(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 ERROR LOADING INDEX: {ex.Message}");
                return View("Error");
            }
        }

        // 🔥 GET: Product/Add
        public IActionResult Add()
        {
            try
            {
                Console.WriteLine("Navigating to Add Product page...");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 ERROR LOADING ADD VIEW: {ex.Message}");
                return View("Error");
            }
        }

        // 🔥 POST: Product/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var farmerId = HttpContext.Session.GetInt32("FarmerId");

                    if (farmerId == null)
                    {
                        Console.WriteLine("⛔ FarmerId is missing in the session.");
                        ModelState.AddModelError("", "You are not authorized to add products.");
                        return View(product);
                    }

                    product.FarmerId = farmerId.Value;

                    // 🔥 Add product and save changes
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    Console.WriteLine("✅ Product saved successfully!");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine($"🔥 Validation Error: {error.ErrorMessage}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 ERROR SAVING PRODUCT: {ex.Message}");
            }

            Console.WriteLine("⛔ Model State Invalid");
            return View(product);
        }

        // 🔥 GET: Product/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var farmerId = HttpContext.Session.GetInt32("FarmerId");

                // 🔥 Fetch the product
                var product = _context.Products
                    .Include(p => p.Farmer)
                    .FirstOrDefault(p => p.Id == id);

                // 🔥 Security check: Farmer can only view their own products
                if (role == "Farmer" && product.FarmerId != farmerId)
                {
                    Console.WriteLine("⛔ Unauthorized access attempt detected.");
                    return RedirectToAction("AccessDenied", "Home");
                }

                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 ERROR LOADING PRODUCT DETAILS: {ex.Message}");
                return View("Error");
            }
        }
    }
}
