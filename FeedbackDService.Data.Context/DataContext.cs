using FeedbackDService.Data.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeedbackDService.Data.Context;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(options => options.MigrationsAssembly("FeedbackDService"));
    }

    public DbSet<CompanyCategory> CompanyCategories => Set<CompanyCategory>();

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<ProductCategory> ProductsCategories => Set<ProductCategory>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<User> Users => Set<User>();
}