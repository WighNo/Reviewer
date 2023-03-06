using Microsoft.AspNetCore.Http;

namespace Reviewer.Data.Responses.Errors.NotFound;

public class ProductNotFound : CustomErrorBase
{
    public ProductNotFound(int productId)
    {
        Content = CreateErrorContent($"Товар с ID {productId} не найден");
    }

    public override CustomErrorContent Content { get; }

    public override int StatusCode => StatusCodes.Status404NotFound;
}