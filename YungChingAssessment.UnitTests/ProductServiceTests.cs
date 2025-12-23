using Moq;
using Xunit;
using YungChingAssessment.Core.Entities;
using YungChingAssessment.Core.Interfaces;
using YungChingAssessment.Core.Services;
using YungChingAssessment.Core.Models;

namespace YungChingAssessment.UnitTests;

public class ProductServiceTests
{
    private readonly Mock<IRepository<Product>> _mockRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _mockRepo = new Mock<IRepository<Product>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _service = new ProductService(_mockRepo.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetPriceDetailsAsync_ThrowsException_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = 99;
        _mockRepo.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetPriceDetailsAsync(productId));
    }

    [Fact]
    public async Task GetPriceDetailsAsync_ReturnsOriginalPrice_WhenPriceIsLow()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Price = 500 };
        _mockRepo.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(product);

        // Act
        var result = await _service.GetPriceDetailsAsync(productId);

        // Assert
        Assert.Equal(500, result.OriginalPrice);
        Assert.Equal(500, result.FinalPrice);
        Assert.False(result.HasDiscount);
    }

    [Fact]
    public async Task GetPriceDetailsAsync_ReturnsDiscountedPrice_WhenPriceIsHigh()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Price = 2000 };
        _mockRepo.Setup(repo => repo.GetByIdAsync(productId))
            .ReturnsAsync(product);

        // Act
        var result = await _service.GetPriceDetailsAsync(productId);

        // Assert
        Assert.Equal(2000, result.OriginalPrice);
        Assert.Equal(1800, result.FinalPrice); // 2000 * 0.9 = 1800
        Assert.True(result.HasDiscount);
    }

    [Fact]
    public async Task CreateProductAsync_CallsAddAndSave()
    {
        // Arrange
        var product = new Product { Name = "New Product", Price = 100 };

        // Act
        await _service.CreateProductAsync(product);

        // Assert
        _mockRepo.Verify(repo => repo.AddAsync(product), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}
