using Microsoft.AspNetCore.Http;

namespace Reviewer.Data.Responses.Errors.NotFound;

/// <summary>
/// Компания не найдена
/// </summary>
public class CompanyByIdNotFound : CustomErrorBase
{
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="companyId"></param>
    public CompanyByIdNotFound(int companyId)
    {
        Content = CreateErrorContent($"Не удалось найти компанию с id {companyId}");
    }

    /// <summary>
    /// Описание ошибки
    /// </summary>
    public override CustomErrorContent Content { get; }

    /// <summary>
    /// Код ответа
    /// </summary>
    public override int StatusCode => StatusCodes.Status404NotFound;
}