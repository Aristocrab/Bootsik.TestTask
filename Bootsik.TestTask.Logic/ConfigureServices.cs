using Bootsik.TestTask.Logic.Database;
using Bootsik.TestTask.Logic.Services;
using Bootsik.TestTask.Logic.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bootsik.TestTask.Logic;

public static class ConfigureServices
{
    public static void AddLogicServices(this IHostApplicationBuilder builder)
    {
        // Db
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
        builder.Services.AddScoped<DbSeeder>();
        
        // FluentValidation
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        builder.Services.AddValidatorsFromAssemblyContaining<HtmlTemplateDtoValidator>();
        
        // Services
        builder.Services.AddScoped<ITemplatesService, TemplatesService>();
    }
}