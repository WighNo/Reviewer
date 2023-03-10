namespace Reviewer.Data.Context.Entities;

public class Company : EntityBase
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string Founder { get; set; } = null!;
    
    public DateTime FoundationDate { get; set; }

    public List<CompanyCategory> Categories { get; set; } = new();
}