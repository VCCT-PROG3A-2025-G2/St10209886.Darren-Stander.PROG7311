// Start of file
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testApp.Services;
using TestApp.Data;
using TestApp.Models;


namespace TestApp.Controllers
{
    // Start of ProductController class
    public class ProductController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IProductService _productService;

        public ProductController(ApplicationContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        // GET: /Product/Index
        // Retrieves products based on user role:
        // - Farmers see only their own products.
        // - Employees can filter by category and production date range.
        // - Others are redirected to AccessDenied.
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
                    // Restrict farmers to their own products
                    products = products.Where(p => p.FarmerId == farmerId.Value);
                }
                else if (role == "Employee")
                {
                    // Apply filters for employees
                    if (!string.IsNullOrEmpty(category))
                        products = products.Where(p => p.Category.Contains(category));
                    if (startDate.HasValue)
                        products = products.Where(p => p.ProductionDate >= startDate.Value);
                    if (endDate.HasValue)
                        products = products.Where(p => p.ProductionDate <= endDate.Value);
                }
                else
                {
                    // Deny access for any other roles
                    return RedirectToAction("AccessDenied", "Home");
                }

                return View(products.ToList());
            }
            catch (Exception ex)
            {
                // Log and show error view on exception
                Console.WriteLine($"ERROR LOADING PRODUCTS: {ex.Message}");
                return View("Error");
            }
        }

        // GET: /Product/Details/{id}
        // Employees only: shows one product and its farmer details.
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

        // GET: /Product/Add
        // Farmers only: displays form to create a new product.
        public IActionResult Add()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Farmer")
                return RedirectToAction("AccessDenied", "Home");

            return View(new Product());
        }

        // POST: /Product/Add
        // Farmers only: sets FarmerId from session, saves new product, redirects to Index.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product)
        {
            try
            {
                var farmerId = HttpContext.Session.GetInt32("FarmerId");
                if (farmerId == null)
                    return RedirectToAction("AccessDenied", "Home");

                product.FarmerId = farmerId.Value;
                await _productService.CreateProductAsync(product);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR ADDING PRODUCT: {ex.Message}");
                return View(product);
            }
        }


        // GET: /Product/Edit/{id}
        // Farmers only: loads form to edit their own product; denies access otherwise.
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

        // POST: /Product/Edit
        // Farmers only: updates product if owned by current farmer and model is valid.
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

        // GET: /Product/Delete/{id}
        // Farmers only: displays confirmation for deleting their own product.
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

        // POST: /Product/DeleteConfirmed/{id}
        // Farmers only: removes the specified product if owned by the current farmer.
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
    // End of ProductController class
}
// End of file
