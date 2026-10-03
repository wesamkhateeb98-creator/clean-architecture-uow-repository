using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Abstracts;
using Shop.Infrastructure.Persistence;
using Shop.Infrastructure.Repositories;

namespace Shop.Infrastructure.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection RegisterInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .RegisterDatabase(configuration)
            .AddScoped<IUnitOfWork, UnitOfWork>();

    private static IServiceCollection RegisterDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Database")));
}
