// Controllers/ProductController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TestApp.Data;
using TestApp.Models;
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

        // GET: Product/Index
        public IActionResult Index(string category, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var role = HttpContext.Session.GetString("Role");
                var farmerId = HttpContext.Session.GetInt32("FarmerId");

                var products = _context.Products
                    .Include(p => p.Farmer)
                    .AsQueryable();

                if (role == "Farmer" && farmerId.HasValue)
                {
                    // Farmers see only their own products
                    products = products.Where(p => p.FarmerId == farmerId.Value);
                }
                else if (role == "Employee")
                {
                    // Employees can filter all products
                    if (!string.IsNullOrEmpty(category))
                        products = products.Where(p => p.Category.Contains(category));
                    if (startDate.HasValue)
                        products = products.Where(p => p.ProductionDate >= startDate.Value);
                    if (endDate.HasValue)
                        products = products.Where(p => p.ProductionDate <= endDate.Value);
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Home");
                }

                return View(products.ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR LOADING PRODUCTS: {ex.Message}");
                return View("Error");
            }
        }

        // GET: Product/Details/5 (Employees only)
        public IActionResult Details(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Employee")
                return RedirectToAction("AccessDenied", "Home");

            var product = _context.Products
                                  .Include(p => p.Farmer)
                                  .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // GET: Product/Add (Farmers only)
        public IActionResult Add()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Farmer")
                return RedirectToAction("AccessDenied", "Home");

            return View(new Product());
        }

        // POST: Product/Add (Farmers only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Product product)
        {
            try
            {
                var farmerId = HttpContext.Session.GetInt32("FarmerId");
                if (farmerId == null)
                    return RedirectToAction("AccessDenied", "Home");

                product.FarmerId = farmerId.Value;

                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR ADDING PRODUCT: {ex.Message}");
                return View(product);
            }
        }

        // GET: Product/Edit/5 (Farmer only, own products)
        public IActionResult Edit(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            var farmerId = HttpContext.Session.GetInt32("FarmerId");

            if (role != "Farmer" || farmerId == null)
                return RedirectToAction("AccessDenied", "Home");

            var product = _context.Products
                                  .FirstOrDefault(p => p.Id == id && p.FarmerId == farmerId);
            if (product == null)
                return RedirectToAction("AccessDenied", "Home");

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            if (farmerId == null || product.FarmerId != farmerId)
                return RedirectToAction("AccessDenied", "Home");

            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Product/Delete/5 (Farmer only, own products)
        public IActionResult Delete(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            var farmerId = HttpContext.Session.GetInt32("FarmerId");

            if (role != "Farmer" || farmerId == null)
                return RedirectToAction("AccessDenied", "Home");

            var product = _context.Products
                                  .FirstOrDefault(p => p.Id == id && p.FarmerId == farmerId);
            if (product == null)
                return RedirectToAction("AccessDenied", "Home");

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var farmerId = HttpContext.Session.GetInt32("FarmerId");
            var product = _context.Products
                                   .FirstOrDefault(p => p.Id == id && p.FarmerId == farmerId);

            if (product == null)
                return RedirectToAction("AccessDenied", "Home");

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
