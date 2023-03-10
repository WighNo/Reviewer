using Microsoft.AspNetCore.Http;

namespace Reviewer.Data.Responses.Errors.NotFound;

/// <summary>
/// 
/// </summary>
public class FormFileNotFound : CustomErrorBase
{
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="nameOfForm">nameof(Self)</param>
    public FormFileNotFound(string nameOfForm)
    {
        Content = CreateErrorContent($"Не удалось найти FormFile {nameOfForm}");
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