using Microsoft.AspNetCore.Http;

namespace FeedbackDService.Services.FileSaveService;

public interface IFileSaveService<T>
{
    Task<T> SaveAsync(IFormFile formFile, string saveFolder);
}