using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Abstract.Services;
using Shop.Application.Services;

namespace Shop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}
