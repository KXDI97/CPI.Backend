using CatalogService.Application.Clients;
using CatalogService.Application.Products;
using CatalogService.Application.Suppliers;
using CatalogService.Infrastructure.Data;
using CatalogService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<CpiDbContext>(opt =>
        {
            var migrations = typeof(CpiDbContext).Assembly.FullName;
            if (OperatingSystem.IsMacOS() || OperatingSystem.IsLinux())
                opt.UseNpgsql(
                    configuration.GetConnectionString("PostgreSQL"),
                    sql => sql.MigrationsAssembly(migrations));
            else
                opt.UseSqlServer(
                    configuration.GetConnectionString("SqlServer"),
                    sql => sql.MigrationsAssembly(migrations));
        });

        // Servicios
        services.AddScoped<IClientService,   ClientService>();
        services.AddScoped<IProductService,  ProductService>();
        services.AddScoped<ISupplierService, SupplierService>();

        return services;
    }
}