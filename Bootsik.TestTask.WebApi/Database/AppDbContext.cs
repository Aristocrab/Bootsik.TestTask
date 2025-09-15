using Bootsik.TestTask.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bootsik.TestTask.WebApi.Database;

public class AppDbContext : DbContext
{
    public DbSet<HtmlTemplate> HtmlTemplates { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var baseEntities = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in baseEntities)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}