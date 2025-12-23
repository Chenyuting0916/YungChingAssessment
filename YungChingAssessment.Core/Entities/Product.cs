namespace YungChingAssessment.Core.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    
    // Foreign Key
    public int CategoryId { get; set; }
    
    // Navigation property
    public Category? Category { get; set; }
}
