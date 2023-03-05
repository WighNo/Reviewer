﻿using FeedbackDService.Responses.Errors;
using Microsoft.AspNetCore.Http;

namespace CoffeeExchange.Data.Responses.Errors.NotFound;

public class FormFileNotFound : CustomErrorBase
{
    public FormFileNotFound(string formFileKey)
    {
        Content = CreateErrorContent($"Не удалось найти FormFile по ключу {formFileKey}");
    }
    
    public override CustomErrorContent Content { get; }
    
    public override int StatusCode => StatusCodes.Status404NotFound;
}