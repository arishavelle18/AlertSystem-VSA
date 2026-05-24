using AlertSystem.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace AlertSystem.API.Common.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {   
    }

    public DbSet<AlertItem> AlertItems { get; set; }
    public IQueryable<AlertItem> AlertItemView => AlertItems.AsNoTracking();
}

