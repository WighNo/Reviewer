using FeedbackDService.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackDService.Controllers;

/// <summary>
/// Контроллер каталога
/// </summary>
[ApiController]
[Route("catalog")]
public class CatalogController : ControllerBase
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="dataContext"></param>
    public CatalogController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// Получить все компании
    /// </summary>
    /// <returns></returns>
    [HttpGet("companies/all")]
    public IActionResult GetAll()
    {
        return Ok(_dataContext.Companies);
    }

    /// <summary>
    /// Получить компанию по ID
    /// </summary>
    /// <param name="id">ID компании</param>
    /// <returns></returns>
    [HttpGet("companies/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var company = await _dataContext.Companies
            .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (company is null)
            return BadRequest();

        return Ok(company);
    }

    /// <summary>
    /// Получить товары и услуги компании
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    [HttpGet("companies/{companyId:int}/products/all")]
    public IActionResult GetProductsByCompany(int companyId)
    {
        var result = _dataContext.Products
            .AsNoTracking()
            .Include(x => x.Reviews)
            .Include(x => x.Categories)
            .Where(x => x.Owner.Id == companyId);

        return Ok(result);
    }

    /// <summary>
    /// Получить товар по ID
    /// </summary>
    /// <param name="productId">ID продукта</param>
    /// <returns></returns>
    [HttpGet("products/{productId:int}")]
    public async Task<IActionResult> GetProductById(int productId)
    {
        var result = await _dataContext.Products
            .AsNoTracking()
            .Where(x => x.Id == productId)
            .Include(x => x.Categories)
            .Include(x => x.Reviews)
            .SingleOrDefaultAsync();

        if (result is null)
            return BadRequest();

        return Ok(result);
    }
}