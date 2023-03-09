using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reviewer.Configs;
using Reviewer.Data.Context;
using Reviewer.Data.Context.Entities;
using Reviewer.Data.Models;
using Reviewer.Data.Requests;
using Reviewer.Data.Responses.Errors.NotFound;
using Reviewer.Helpers;
using Reviewer.Services.FileSaveService;

namespace Reviewer.Controllers;

/// <summary>
/// Контроллер товаров
/// </summary>
[ApiController]
[Route("product")]
public class ProductController : ControllerBase
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IFileSaveService<string> _fileSaveService;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="dataContext">Провайдер данных</param>
    /// <param name="mapper">Сопоставитель данных</param>
    /// <param name="fileSaveService">Сервис сохранения картинок</param>
    public ProductController(DataContext dataContext, IMapper mapper, ImageFileSaveService fileSaveService)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _fileSaveService = fileSaveService;
    }

    /// <summary>
    /// Получить топ-10 рецензий
    /// </summary>
    /// <returns></returns>
    [HttpGet("top-ten")]
    public IActionResult TopTenReviews()
    {
        var reviews = _dataContext.Reviews
            .AsNoTracking()
            .OrderByDescending(x => x.Likes!.Count)
            .Take(10);

        return Ok(reviews);
    }
    
    /// <summary>
    /// Добавить рецензию товару
    /// </summary>
    /// <param name="productId">ID товара</param>
    /// <param name="request">Параметры</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("add-review/{productId:int}")]
    [ProducesResponseType(typeof(List<Review>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(UserNotFound), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProductNotFound), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddReview(int productId, [FromBody] CreateReviewRequest request)
    {
        var userId = HttpContext.GetUserIdClaim();
        var user = await _dataContext.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user is null)
            return new UserNotFound(userId);
        
        var product = await _dataContext.Products
            .Where(x => x.Id == productId)
            .Include(x => x.Reviews)
            .SingleOrDefaultAsync();
        
        if(product is null)
            return new ProductNotFound(productId);

        var review = _mapper.Map<Review>(request);
        review.Owner = user;

        var reviewEntry = await _dataContext.Reviews.AddAsync(review);
        
        product.Reviews ??= new List<Review>();
        product.Reviews.Add(reviewEntry.Entity);

        var productEntry = _dataContext.Products.Update(product);

        return Ok(productEntry.Entity.Reviews);
    }

    /// <summary>
    /// Добавить товар или услугу
    /// </summary>
    /// <param name="request">Параметры</param>
    /// <returns></returns>
    [Authorize(Roles = UserRoles.Admin)]
    [HttpPost("add-new")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CompanyByIdNotFound), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddNew([FromForm] AddProductRequest request)
    {
        var product = _mapper.Map<Product>(request);
        
        if(request.Image is not null)
            product.ImageUrl = await _fileSaveService.SaveAsync(request.Image, SavePathsConfig.ProductsImages);

        var company = await _dataContext.Companies
            .FirstOrDefaultAsync(x => x.Id == request.OwnerId);
        
        if (company is null)
            return new CompanyByIdNotFound(request.OwnerId);

        var categories = await _dataContext.ProductsCategories
            .Where(x => request.CategoriesIds.Contains(x.Id))
            .ToListAsync();

        product.Owner = company;
        product.Categories = categories;

        var entry = await _dataContext.Products.AddAsync(product);
        await _dataContext.SaveChangesAsync();
        
        return Ok(entry.Entity);
    } 
}