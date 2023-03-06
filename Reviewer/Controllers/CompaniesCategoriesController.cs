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
    public async Task<IActionResult> CreateNew([FromForm] CreateCompanyCategoryRequest request)
    {
        var category = _mapper.Map<CompanyCategory>(request);

        if (_dataContext.CompanyCategories.Any(x => x.Name == category.Name))
            return BadRequest();

        await _dataContext.CompanyCategories.AddAsync(category);
        await _dataContext.SaveChangesAsync();

        return Ok(category);
    }
}