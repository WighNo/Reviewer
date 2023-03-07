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