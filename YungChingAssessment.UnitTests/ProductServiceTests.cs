using Moq;
using Xunit;
using YungChingAssessment.Core.Entities;
using YungChingAssessment.Core.Interfaces;
using YungChingAssessment.Core.Services;

namespace YungChingAssessment.UnitTests;

public class ProductServiceTests
{
    private readonly Mock<IRepository<Product>> _mockRepo;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _mockRepo = new Mock<IRepository<Product>>();
        _service = new ProductService(_mockRepo.Object);
    }


    
    // REMOVED: Simple CRUD tests (GetById, Create success path) as they test Mock behavior, not business logic.
    // Focusing only on Validation and Calculation logic as requested.

    [Fact]
    public async Task CreateProductAsync_ThrowsException_WhenPriceIsInvalid()
    {
        // Arrange
        var product = new Product { Name = "Invalid Product", Price = -10 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateProductAsync(product));
        _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task GetDiscountedPriceAsync_ReturnsOriginalPrice_WhenPriceIsLow()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Price = 500 };
        _mockRepo.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(product);

        // Act
        var result = await _service.GetDiscountedPriceAsync(productId);

        // Assert
        Assert.Equal(500, result);
    }

    [Fact]
    public async Task GetDiscountedPriceAsync_ReturnsDiscountedPrice_WhenPriceIsHigh()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Price = 2000 };
        _mockRepo.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(product);

        // Act
        var result = await _service.GetDiscountedPriceAsync(productId);

        // Assert
        Assert.Equal(1800, result); // 2000 * 0.9 = 1800
    }
}
