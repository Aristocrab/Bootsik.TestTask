using Bootsik.TestTask.WebApi.Database;
using Bootsik.TestTask.WebApi.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Bootsik.TestTask.WebApi;

public static class ConfigureServices
{
    public static void AddApiServices(this IHostApplicationBuilder builder)
    {
        // Serilog
        var logger = Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Services.AddSerilog(logger);
        
        // Controllers
        builder.Services.AddControllers();
        
        // Swagger
        builder.Services.AddSwaggerGen();
        
        // Db
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
        builder.Services.AddScoped<DbSeeder>();
        
        // FluentValidation
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        
        // Services
        builder.Services.AddScoped<ITemplatesService, TemplatesService>();
    }
}