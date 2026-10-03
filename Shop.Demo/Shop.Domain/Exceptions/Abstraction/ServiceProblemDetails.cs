namespace Shop.Domain.Exceptions.Abstraction;

public class ServiceProblemDetails
{
    public required string Title { get; init; }
    public string? Detail { get; init; }
    public required string Type { get; init; }
    public string? Instance { get; init; }
    public IDictionary<string, object?> Extensions { get; init; }
        = new Dictionary<string, object?>(StringComparer.Ordinal);
}
