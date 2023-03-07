using Microsoft.AspNetCore.Http;

namespace Reviewer.Data.Responses.Errors.NotFound;

/// <summary>
/// 
/// </summary>
public class FormFileByKeyNotFound : CustomErrorBase
{
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="formFileKey">Ключ к FormFile'у</param>
    public FormFileByKeyNotFound(string formFileKey)
    {
        Content = CreateErrorContent($"Не удалось найти FormFile по ключу {formFileKey}");
    }
    
    /// <summary>
    /// Описание
    /// </summary>
    public override CustomErrorContent Content { get; }
    
    /// <summary>
    /// Код ответа
    /// </summary>
    public override int StatusCode => StatusCodes.Status404NotFound;
}