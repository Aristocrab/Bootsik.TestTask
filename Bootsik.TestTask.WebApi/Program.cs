using Bootsik.TestTask.WebApi;
using Bootsik.TestTask.WebApi.Database;
using Bootsik.TestTask.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiServices();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<DbSeeder>();
await dbContext.SeedAsync();

app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();