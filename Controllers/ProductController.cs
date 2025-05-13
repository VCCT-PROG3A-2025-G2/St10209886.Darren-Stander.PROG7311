using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApp.Models;
using TestApp.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

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
        public IActionResult Index(string category, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var farmerId = HttpContext.Session.GetInt32("FarmerId");

                // 🔍 Load all products, including their linked Farmer
                var products = _context.Products
                    .Include(p => p.Farmer)
                    .AsQueryable();

                // 🔥 Filter for Farmers - show only their own products
                if (role == "Farmer" && farmerId.HasValue)
                {
                    products = products.Where(p => p.FarmerId == farmerId.Value);
                }

                // 🔥 Filter for Employees based on query
                if (role == "Employee")
                {
                    if (!string.IsNullOrEmpty(category))
                    {
                        products = products.Where(p => p.Category.Contains(category));
                    }

                    if (startDate.HasValue)
                    {
                        products = products.Where(p => p.ProductionDate >= startDate.Value);
                    }

                    if (endDate.HasValue)
                    {
                        products = products.Where(p => p.ProductionDate <= endDate.Value);
                    }
                }

                return View(products.ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 ERROR LOADING INDEX: {ex.Message}");
                return View("Error");
            }
        }

        // 🔥 GET: Product/Details/5 (Employee ONLY)
        public IActionResult Details(int id)
        {
            var role = HttpContext.Session.GetString("Role");

            // 🔥 Only Employees can view details
            if (role != "Employee")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            // 🔍 Fetch the product and include Farmer reference
            var product = _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // 🔥 GET: Product/Add
        public IActionResult Add()
        {
            var role = HttpContext.Session.GetString("Role");

            // 🔥 Only Farmers can add
            if (role != "Farmer")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View(new Product());
        }

        // 🔥 POST: Product/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Product product)
        {
            try
            {
                var farmerId = HttpContext.Session.GetInt32("FarmerId");

                if (farmerId == null)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }

                product.FarmerId = farmerId.Value;

                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 ERROR ADDING PRODUCT: {ex.Message}");
                return View(product);
            }
        }

        // 🔥 GET: Product/Edit/5
        public IActionResult Edit(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Farmer")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null || product.FarmerId != farmerId)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View(product);
        }

        // 🔥 POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            var farmerId = HttpContext.Session.GetInt32("FarmerId");

            if (product.FarmerId != farmerId)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // 🔥 GET: Product/Delete/5
        public IActionResult Delete(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Farmer")
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null || product.FarmerId != farmerId)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View(product);
        }

        // 🔥 POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var farmerId = HttpContext.Session.GetInt32("FarmerId");

            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null || product.FarmerId != farmerId)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
