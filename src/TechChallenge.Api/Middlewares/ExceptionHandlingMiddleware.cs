using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace TechChallenge.Api.Middlewares;
// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
[ExcludeFromCodeCoverage]
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred: {Message}", ex.Message);

            var problemDetails = new ProblemDetails
            {
                Title = "Business Error",
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                Detail = ex.Message,
            };

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
[ExcludeFromCodeCoverage]
public static class GlobalResponseMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
