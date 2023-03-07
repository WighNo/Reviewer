using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Reviewer.Data.Responses.Errors;

/// <summary>
/// Базовый класс для вывода ошибок
/// </summary>
public abstract class CustomErrorBase : IActionResult
{
    /// <summary>
    /// Описание ошибки
    /// </summary>
    public abstract CustomErrorContent Content { get; }
    
    /// <summary>
    /// Код ошибки
    /// </summary>
    public abstract int StatusCode { get; }
    
    private static Dictionary<int, string> StatusCodesTitles => new()
    {
        {StatusCodes.Status400BadRequest, "Bad Request"},
        {StatusCodes.Status404NotFound, "Not Found"}
    };

    /// <summary>
    /// Отправка ответа
    /// </summary>
    /// <param name="context"></param>
    public async Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(Content)
        {
            StatusCode = StatusCode
        };

        await objectResult.ExecuteResultAsync(context);
    }

    /// <summary>
    /// Метод для создания описания ошибки
    /// </summary>
    /// <param name="content">Описание</param>
    /// <returns></returns>
    public CustomErrorContent CreateErrorContent(string content)
    {
        return new CustomErrorContent(StatusCodesTitles[StatusCode], content);
    }
}