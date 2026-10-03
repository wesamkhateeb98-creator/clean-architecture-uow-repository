using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Abstracts.Services;
using Shop.Application.Services;

namespace Shop.Application.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
        => services
            .AddScoped<IProductService, ProductService>()
            .AddScoped<ICategoryService, CategoryService>();
}
