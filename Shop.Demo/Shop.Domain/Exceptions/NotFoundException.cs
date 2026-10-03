using Shop.Domain.Exceptions.Abstraction;

namespace Shop.Domain.Exceptions;

public class NotFoundException(string message) : Exception(message), IProblemDetailsProvider
{
    public ServiceProblemDetails GetProblemDetails() => new()
    {
        Title = Message,
        Type = "Not Found"
    };
}
