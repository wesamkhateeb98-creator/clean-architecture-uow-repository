namespace Shop.Domain.Exceptions.Abstraction;

public interface IProblemDetailsProvider
{
    ServiceProblemDetails GetProblemDetails();
}
