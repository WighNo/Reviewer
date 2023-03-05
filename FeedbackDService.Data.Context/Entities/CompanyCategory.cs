namespace FeedbackDService.Data.Context.Entities;

public class CompanyCategory : EntityBase
{
    public const string NoneCategoryName = "None";
    
    public string Name { get; set; } = null!;
}