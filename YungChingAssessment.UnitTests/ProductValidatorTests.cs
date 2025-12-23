using YungChingAssessment.Api.DTOs;
using YungChingAssessment.Api.Validation;

namespace YungChingAssessment.UnitTests;

public class ProductValidatorTests
{
    private readonly CreateProductDtoValidator _createValidator;
    private readonly UpdateProductDtoValidator _updateValidator;

    public ProductValidatorTests()
    {
        _createValidator = new CreateProductDtoValidator();
        _updateValidator = new UpdateProductDtoValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CreateProductDto_ShouldHaveError_WhenNameIsEmpty(string name)
    {
        var dto = new CreateProductDto { Name = name, Price = 100 };
        var result = _createValidator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void CreateProductDto_ShouldHaveError_WhenNameExceeds200Characters()
    {
        var dto = new CreateProductDto { Name = new string('A', 201), Price = 100 };
        var result = _createValidator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void CreateProductDto_ShouldHaveError_WhenPriceIsZeroOrLess()
    {
        var dto = new CreateProductDto { Name = "Test", Price = 0 };
        var result = _createValidator.Validate(dto);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void CreateProductDto_ShouldBeValid_WhenDataIsCorrect()
    {
        var dto = new CreateProductDto { Name = "Valid Product", Price = 100 };
        var result = _createValidator.Validate(dto);
        Assert.True(result.IsValid);
    }
}
