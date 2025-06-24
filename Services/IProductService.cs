using TestApp.Models;

namespace testApp.Services
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetAllProductsAsync();
        Task DeleteProductAsync(int id);
    }
}