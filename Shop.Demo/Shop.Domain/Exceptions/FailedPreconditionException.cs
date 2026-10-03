using Shop.Domain.Exceptions.Abstraction;

namespace Shop.Domain.Exceptions;

public class FailedPreconditionException(string message) : Exception(message), IProblemDetailsProvider
{
    public ServiceProblemDetails GetProblemDetails() => new()
    {
        Title = Message,
        Type = "Failed Precondition"
    };
}
