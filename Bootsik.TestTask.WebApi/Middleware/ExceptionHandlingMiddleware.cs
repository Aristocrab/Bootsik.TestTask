using System.Net;
using Bootsik.TestTask.Logic.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Bootsik.TestTask.WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", ex.Message);
            
            var statusCode = ex switch
            {  
                NotFoundException => (int)HttpStatusCode.NotFound,     
                ValidationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var message = ex.Message;

            if (ex is ValidationException validationException)
            {
                message = validationException.Errors.First().ErrorMessage;
            }
            
            var problemDetails = new ProblemDetails
            {
                Title = ex.GetType().Name,
                Detail = message,
                Status = statusCode,
                Instance = context.Request.Path
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}