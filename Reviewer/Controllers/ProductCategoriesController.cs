using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reviewer.Data.Context;
using Reviewer.Data.Context.Entities;
using Reviewer.Data.Models;
using Reviewer.Data.Requests;

namespace Reviewer.Controllers;

/// <summary>
/// Контроллер категорий товаров
/// </summary>
[ApiController]
[Route("product-categories")]
public class ProductCategoriesController : ControllerBase
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="dataContext">Провайдер данных</param>
    /// <param name="mapper">Сопоставитель данных</param>
    public ProductCategoriesController(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    /// <summary>
    /// Добавить новую категорию для товара
    /// </summary>
    /// <param name="request">Параметры</param>
    /// <returns></returns>
    [Authorize(Roles = UserRoles.Admin)]
    [HttpPost("add-new")]
    [ProducesResponseType(typeof(ProductCategory), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddNew([FromBody] AddProductCategoryRequest request)
    {
        var productCategory = _mapper.Map<ProductCategory>(request);
        var entry = await _dataContext.ProductsCategories.AddAsync(productCategory);
        await _dataContext.SaveChangesAsync();
        return Ok(entry.Entity);
    }
}