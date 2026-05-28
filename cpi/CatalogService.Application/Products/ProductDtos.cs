namespace CatalogService.Application.Products;

public record ProductDto(
    string ProductId,
    string Name,
    decimal Value,
    string Category,
    string Description,
    decimal Stock,
    int SupplierId
);

public record CreateProductDto(
    string ProductId,
    string Name,
    decimal Value,
    string Category,
    string Description,
    decimal Stock,
    int SupplierId
);

public record UpdateProductDto(
    string Name,
    decimal Value,
    string Category,
    string Description,
    decimal Stock,
    int SupplierId
);