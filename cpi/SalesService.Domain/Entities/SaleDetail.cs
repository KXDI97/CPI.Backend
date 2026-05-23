namespace SalesService.Domain.Entities;

public class SaleDetail
{
    public int     InvoiceDetailId { get; set; }
    public int     InvoiceId       { get; set; }
    public string  ProductId       { get; set; } = default!;
    public decimal Quantity        { get; set; }
    public decimal UnitPrice       { get; set; }
    public decimal LineTotal       { get; set; }  // columna calculada en BD (PERSISTED)

    public Sale Sale { get; set; } = default!;
}