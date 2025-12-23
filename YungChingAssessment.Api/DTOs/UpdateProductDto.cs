namespace YungChingAssessment.Api.DTOs;

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool IsActive { get; set; }
}
