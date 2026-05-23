namespace SalesService.Domain.Entities;

public class Sale
{
    public int       InvoiceId    { get; set; }
    public int       ClientId     { get; set; }
    public DateTime  InvoiceDate  { get; set; }
    public string    Status       { get; set; } = "Emitida";
    public decimal?  ExchangeRate { get; set; }
    public decimal   Subtotal     { get; set; }
    public decimal   Tax          { get; set; }
    public decimal   Total        { get; set; }

    public ICollection<SaleDetail> Details { get; set; } = new List<SaleDetail>();
}