using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesService.Application.Sales;
using SalesService.Infrastructure.Data;
using SalesService.Infrastructure.Services;

namespace SalesService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SalesDbContext>(opt =>
        {
            var migrations = typeof(SalesDbContext).Assembly.FullName;
            if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
                opt.UseNpgsql(
                    configuration.GetConnectionString("PostgreSQL"),
                    sql => sql.MigrationsAssembly(migrations));
            else
                opt.UseSqlServer(
                    configuration.GetConnectionString("SqlServer"),
                    sql => sql.MigrationsAssembly(migrations));
        });

        services.AddScoped<ISaleService, SaleService>();

        return services;
    }
}