using AutoMapper;
using CoffeeExchange.Data.Responses.Errors.NotFound;
using FeedbackDService.Configs;
using FeedbackDService.Data.Context;
using FeedbackDService.Data.Context.Entities;
using FeedbackDService.Data.Models;
using FeedbackDService.Requests;
using FeedbackDService.Services.FileSaveService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackDService.Controllers;

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
    public async Task<IActionResult> Get(int id)
    {
        var company = await _dataContext.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (company is null)
            return BadRequest();
        
        return Ok(company);
    }

    /// <summary>
    /// Получить все компании
    /// </summary>
    /// <returns></returns>
    [HttpGet("all")]
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
    public async Task<IActionResult> AddNew([FromForm] AddCompanyRequest request)
    {
        var formCollection = await HttpContext.Request.ReadFormAsync();
        var photo = formCollection.Files[request.FormImageKey];
        
        if (photo is null)
            return new FormFileNotFound(request.FormImageKey);

        var company = _mapper.Map<Company>(request);
        company.ImageUrl = await _imageSaveService.SaveAsync(photo, SavePathsConfig.CompaniesImages);

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