using System.ComponentModel.DataAnnotations;

namespace CatalogService.Api.Products;

public record ProductDto(
    string ProductId, string Name, decimal Value,
    string Category, string Description, decimal Stock, int SupplierId);

public class CreateUpdateProductDto
{
    [Required, StringLength(50)] public string ProductId { get; set; } = default!;
    [Required, StringLength(100)] public string Name { get; set; } = default!;
    [Range(0, (double)decimal.MaxValue)] public decimal Value { get; set; }
    [Required, StringLength(50)] public string Category { get; set; } = default!;
    [Required, StringLength(255)] public string Description { get; set; } = default!;
    [Range(0, (double)decimal.MaxValue)] public decimal Stock { get; set; }
    [Range(1, int.MaxValue)] public int SupplierId { get; set; }
}

public record ProductsQuery(
    int Page = 1, int PageSize = 12, string? Q = null,
    string? Category = null, int? SupplierId = null, string? Sort = null);

public record PagedResult<T>(int Page, int PageSize, int TotalCount, IEnumerable<T> Items);
