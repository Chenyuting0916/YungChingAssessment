using YungChingAssessment.Core.Entities;
using YungChingAssessment.Core.Interfaces;
using YungChingAssessment.Core.Models;

namespace YungChingAssessment.Core.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }
        return product;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await _productRepository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return product;
    }

    public async Task UpdateProductAsync(int id, Product product)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        existingProduct.IsActive = product.IsActive;
        
        await _productRepository.UpdateAsync(existingProduct);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }

        await _productRepository.DeleteAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ProductPriceDetails> GetPriceDetailsAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }

        var finalPrice = CalculateDiscountedPrice(product.Price);

        return new ProductPriceDetails(
            product.Price,
            finalPrice,
            product.Price != finalPrice
        );
    }

    private decimal CalculateDiscountedPrice(decimal originalPrice)
    {
        if (originalPrice > 1000)
        {
            return originalPrice * 0.9m; // 10% discount
        }

        return originalPrice;
    }
}
