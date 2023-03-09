using Microsoft.AspNetCore.Http;

namespace Reviewer.Data.Requests;

public record AddCompanyRequest(string Name, string Description, string Founder, DateTime FoundationDate, IFormFile Image, List<int>? CategoriesIds);