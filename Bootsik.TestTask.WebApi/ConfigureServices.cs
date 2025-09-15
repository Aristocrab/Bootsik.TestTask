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
    }
}