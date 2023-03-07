using Microsoft.EntityFrameworkCore;
using Reviewer.Data.Context.Entities;

namespace Reviewer.Data.Context;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(options => options.MigrationsAssembly("Reviewer"));
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<CompanyCategory> CompanyCategories => Set<CompanyCategory>();

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<ProductCategory> ProductsCategories => Set<ProductCategory>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Review> Reviews => Set<Review>();

    public DbSet<Like> Likes => Set<Like>();
    
    public DbSet<Dislike> Dislikes => Set<Dislike>();
}