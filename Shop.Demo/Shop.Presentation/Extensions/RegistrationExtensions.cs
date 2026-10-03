namespace Shop.Presentation.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection RegisterPresentation(this IServiceCollection services)
        => services.AddOpenApi();
}
