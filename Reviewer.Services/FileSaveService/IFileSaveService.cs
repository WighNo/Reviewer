using Microsoft.AspNetCore.Http;

namespace Reviewer.Services.FileSaveService;

public interface IFileSaveService<T>
{
    Task<T> SaveAsync(IFormFile formFile, string saveFolder);
}