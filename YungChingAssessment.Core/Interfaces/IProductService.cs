using YungChingAssessment.Core.Entities;
using YungChingAssessment.Core.Models;

namespace YungChingAssessment.Core.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task UpdateProductAsync(int id, Product product);
    Task DeleteProductAsync(int id);
    Task<ProductPriceDetails> GetPriceDetailsAsync(int id);
}
