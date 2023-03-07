using Reviewer.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reviewer.Data.Context.Entities;
using Reviewer.Data.Responses.Errors.NotFound;

namespace Reviewer.Controllers;

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
    [ProducesResponseType(typeof(List<Company>), StatusCodes.Status200OK)]
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
    [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CompanyByIdNotFound), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var company = await _dataContext.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (company is null)
            return new CompanyByIdNotFound(id);

        return Ok(company);
    }

    /// <summary>
    /// Получить товары и услуги компании
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    [HttpGet("companies/{companyId:int}/products/all")]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CompanyByIdNotFound), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductsByCompany(int companyId)
    {
        var company = await _dataContext.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == companyId);

        if (company is null)
            return new CompanyByIdNotFound(companyId);
        
        var result = _dataContext.Products
            .AsNoTracking()
            .Where(x => x.Owner.Id == companyId)
            .Include(x => x.Reviews)
            .Include(x => x.Categories);

        return Ok(result);
    }

    /// <summary>
    /// Получить товар по ID
    /// </summary>
    /// <param name="productId">ID продукта</param>
    /// <returns></returns>
    [HttpGet("products/{productId:int}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProductNotFound), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(int productId)
    {
        var hasProduct = await _dataContext.Products.AnyAsync(x => x.Id == productId);

        if (hasProduct == false)
            return new ProductNotFound(productId);
        
        var product = await _dataContext.Products
            .AsNoTracking()
            .Where(x => x.Id == productId)
            .Include(x => x.Categories)
            .Include(x => x.Owner)
            .Include(x => x.Reviews)
            .SingleOrDefaultAsync();

        if (product is null)
            return new ProductNotFound(productId);

        return Ok(product);
    }
}