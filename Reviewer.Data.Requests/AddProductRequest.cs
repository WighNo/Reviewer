using Microsoft.AspNetCore.Http;

namespace Reviewer.Data.Requests;

public record AddProductRequest(int OwnerId, List<int> CategoriesIds, string Name, string Description, List<string> AdditionalDescription, IFormFile? Image);
