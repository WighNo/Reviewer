namespace FeedbackDService.Data.Context.Entities;

public class Product : EntityBase
{
    public Company Owner { get; set; } = null!;

    public List<ProductCategory>? Categories { get; set; } = new();

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public List<string>? AdditionalDescription { get; set; } = new();
    
    public string? ImageUrl { get; set; }

    public List<Review>? Reviews { get; set; }
}