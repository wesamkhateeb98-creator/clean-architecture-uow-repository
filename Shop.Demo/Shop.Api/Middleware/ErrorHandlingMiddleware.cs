using Shop.Application.Exceptions;

namespace Shop.Api.Middleware;

// Turns Application exceptions into HTTP status codes in one place.
public class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteAsync(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (BadRequestException ex)
        {
            await WriteAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
    }

    private static Task WriteAsync(HttpContext context, int status, string message)
    {
        context.Response.StatusCode = status;
        return context.Response.WriteAsJsonAsync(new { status, error = message });
    }
}
