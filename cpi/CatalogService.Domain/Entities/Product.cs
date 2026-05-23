namespace CatalogService.Domain.Entities;

public class Product
{
    public string ProductId { get; set; } = default!;   // nvarchar(50) PK
    public string Name { get; set; } = default!;
    public decimal Value { get; set; }                  // decimal(19,4)
    public string Category { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Stock { get; set; }                  // decimal(18,4) (acumulado inicial)
    public int SupplierId { get; set; }                 // FK → Suppliers
}
