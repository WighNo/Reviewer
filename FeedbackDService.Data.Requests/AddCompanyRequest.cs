namespace FeedbackDService.Requests;

public record AddCompanyRequest(string Name, string Description, string FormImageKey, List<int>? CategoriesIds);