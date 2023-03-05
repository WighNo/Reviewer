using Microsoft.AspNetCore.Http;

namespace FeedbackDService.Responses.Errors.NotFound;

public class UserNotFound : CustomErrorBase
{
    public UserNotFound(int userId)
    {
        Content = CreateErrorContent($"Пользователь с ID {userId} не найден");
    }
    
    public override CustomErrorContent Content { get; }

    public override int StatusCode => StatusCodes.Status404NotFound;
}