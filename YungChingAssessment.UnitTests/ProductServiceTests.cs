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

    [Fact]
    public async Task GetProductByIdAsync_ReturnsProduct_WhenProductExists()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Name = "Test Product", Price = 100 };
        _mockRepo.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(product);

        // Act
        var result = await _service.GetProductByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal("Test Product", result.Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = 99;
        _mockRepo.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.GetProductByIdAsync(productId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateProductAsync_AddsProduct_AndReturnsIt()
    {
        // Arrange
        var product = new Product { Name = "New Product", Price = 200 };
        _mockRepo.Setup(repo => repo.AddAsync(product))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateProductAsync(product);

        // Assert
        Assert.Equal(product, result);
        _mockRepo.Verify(repo => repo.AddAsync(product), Times.Once);
    }

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
