using Microsoft.AspNetCore.Http;

namespace Reviewer.Data.Requests;

public record AddCompanyRequest(string Name, string Description, IFormFile Image, List<int>? CategoriesIds);