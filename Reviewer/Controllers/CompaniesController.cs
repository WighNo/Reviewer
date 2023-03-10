using AutoMapper;
using Reviewer.Configs;
using Reviewer.Data.Context;
using Reviewer.Data.Context.Entities;
using Reviewer.Data.Models;
using Reviewer.Services.FileSaveService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reviewer.Data.Requests;
using Reviewer.Data.Responses.Errors.NotFound;

namespace Reviewer.Controllers;

/// <summary>
/// Контроллер компаний
/// </summary>
[ApiController]
[Route("companies")]
public class CompaniesController : ControllerBase
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IFileSaveService<string> _imageSaveService;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="dataContext">Провайдер данных</param>
    /// <param name="mapper">Сопоставитель данных</param>
    /// <param name="imageSaveService">Сервис сохранения изображений компаний</param>
    public CompaniesController(DataContext dataContext, IMapper mapper, ImageFileSaveService imageSaveService)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _imageSaveService = imageSaveService;
    }

    /// <summary>
    /// Получить компанию
    /// </summary>
    /// <param name="id">ID компании</param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CompanyByIdNotFound), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(int id)
    {
        var company = await _dataContext.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (company is null)
            return new CompanyByIdNotFound(id);
        
        return Ok(company);
    }

    /// <summary>
    /// Получить все компании
    /// </summary>
    /// <returns></returns>
    [HttpGet("all")]
    [ProducesResponseType(typeof(List<Company>), StatusCodes.Status200OK)]
    public IActionResult All()
    {
        var companies = _dataContext.Companies
            .AsNoTracking()
            .Include(x => x.Categories);
        
        return Ok(companies);
    }

    /// <summary>
    /// Добавить компанию
    /// </summary>
    /// <returns></returns>
    [HttpPost("add")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddNew([FromForm] AddCompanyRequest request)
    {
        var company = _mapper.Map<Company>(request);
        company.FoundationDate = request.FoundationDate.ToUniversalTime();
        company.ImageUrl = await _imageSaveService.SaveAsync(request.Image, SavePathsConfig.CompaniesImages);

        if (request.CategoriesIds is null || request.CategoriesIds.Count == 0)
        {
            var noneCategory = _dataContext.CompanyCategories.FirstOrDefault(x => x.Name == CompanyCategory.NoneCategoryName);
            if (noneCategory is not null) 
                company.Categories.Add(noneCategory);
        }
        else
        {
            var categories = _dataContext.CompanyCategories
                .Where(x => request.CategoriesIds.Contains(x.Id));

            company.Categories.AddRange(categories);
        }

        var result = await _dataContext.Companies.AddAsync(company);
        await _dataContext.SaveChangesAsync();

        return Ok(result.Entity);
    }
}