namespace Reviewer.Data.Responses.Errors;

/// <summary>
/// Содержание ошибки
/// </summary>
public class CustomErrorContent
{
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="title">Заголовок</param>
    /// <param name="content">Содержание</param>
    public CustomErrorContent(string title, string content)
    {
        Title = title;
        Content = content;
    }

    /// <summary>
    /// Заголовок
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Содержание
    /// </summary>
    public string Content { get; }
}