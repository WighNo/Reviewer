using AutoMapper;
using Reviewer.Data.Context;
using Reviewer.Data.Context.Entities;
using Reviewer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reviewer.Data.Requests;

namespace Reviewer.Controllers;

/// <summary>
/// Контроллер категорий компаний
/// </summary>
[ApiController]
[Route("companies-categories")]
public class CompaniesCategoriesController : ControllerBase
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="dataContext">Провайдер данных</param>
    /// <param name="mapper">Сопоставитель данных</param>
    public CompaniesCategoriesController(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить все категории компаний
    /// </summary>
    /// <returns></returns>
    [HttpGet("all")]
    [ProducesResponseType(typeof(List<CompanyCategory>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult All()
    {
        return Ok(_dataContext.CompanyCategories);
    }

    /// <summary>
    /// Создать новую категрию компании
    /// </summary>
    /// <returns></returns>
    [HttpPost("create")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(typeof(CompanyCategory), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateNew([FromForm] CreateCompanyCategoryRequest request)
    {
        var category = _mapper.Map<CompanyCategory>(request);

        if (_dataContext.CompanyCategories.Any(x => x.Name == category.Name))
            return BadRequest();

        var result = await _dataContext.CompanyCategories.AddAsync(category);
        await _dataContext.SaveChangesAsync();

        return Ok(result.Entity);
    }
}