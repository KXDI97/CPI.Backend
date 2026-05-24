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
            opt.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(CpiDbContext).Assembly.FullName)
            ));

        // Servicios
        services.AddScoped<IClientService,   ClientService>();
        services.AddScoped<IProductService,  ProductService>();
        services.AddScoped<ISupplierService, SupplierService>();

        return services;
    }
}