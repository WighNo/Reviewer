using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FeedbackDService.Services.FileSaveService;

public static class FileSaveServiceExtensions
{
    public static IServiceCollection AddFileSaveService<T>(this IServiceCollection services, WebApplicationBuilder builder)
        where T : FilesSaveServiceBase, new()
    {
        services.AddScoped<FilesSaveServiceBase>(_ => new T()
        {
            WebRootDirectoryPath = builder.Environment.WebRootPath,
        });
        
        return services;
    }
}