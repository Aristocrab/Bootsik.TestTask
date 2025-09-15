using Bootsik.TestTask.Logic.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bootsik.TestTask.Logic.Database;

public class DbSeeder
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<DbSeeder> _logger;

    public DbSeeder(AppDbContext dbContext, ILogger<DbSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task SeedAsync()
    {
        await _dbContext.Database.MigrateAsync();
        
        if (!_dbContext.HtmlTemplates.Any())
        {
            var templates = new List<HtmlTemplate>
            {
                new()
                {
                    Name = "ExampleTemplate",
                    Content = "Hello, <b>{{name}}</b>!"
                }
            };

            _dbContext.HtmlTemplates.AddRange(templates);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Database seeded successfully");
        }
    }
}