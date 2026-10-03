using Shop.Domain.Exceptions.Abstraction;

namespace Shop.Presentation.Middleware;

// Turns Domain exceptions into HTTP problem details in one place.
public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex) when (ex is IProblemDetailsProvider provider)
        {
            await WriteError(context, provider.GetProblemDetails());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await WriteError(context, new ServiceProblemDetails { Title = "Unexpected error.", Type = "Internal Server Error" });
        }
    }

    private static Task WriteError(HttpContext context, ServiceProblemDetails problem)
    {
        var status = problem.Type switch
        {
            "Not Found" => StatusCodes.Status404NotFound,
            "Already Exists" => StatusCodes.Status409Conflict,
            "Failed Precondition" => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = status;
        return context.Response.WriteAsJsonAsync(new
        {
            status,
            title = problem.Title,
            type = problem.Type,
            detail = problem.Detail,
            extensions = problem.Extensions
        });
    }
}
