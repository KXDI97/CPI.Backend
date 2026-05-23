namespace SalesService.Application.Sales;

// ── Lectura ──────────────────────────────────────────────────────────────────

public record SaleDetailDto(
    int     InvoiceDetailId,
    string  ProductId,
    decimal Quantity,
    decimal UnitPrice,
    decimal LineTotal
);

public record SaleDto(
    int                    InvoiceId,
    int                    ClientId,
    string                 ClientName,
    DateTime               InvoiceDate,
    string                 Status,
    decimal?               ExchangeRate,
    decimal                Subtotal,
    decimal                Tax,
    decimal                Total,
    IEnumerable<SaleDetailDto> Details
);

public record SaleSummaryDto(
    int      InvoiceId,
    int      ClientId,
    string   ClientName,
    DateTime InvoiceDate,
    string   Status,
    decimal  Total
);

// ── Escritura ────────────────────────────────────────────────────────────────

public record CreateSaleLineDto(
    string  ProductId,
    decimal Quantity,
    decimal UnitPrice
);

public record CreateSaleDto(
    int                        ClientId,
    DateTime                   InvoiceDate,
    decimal?                   ExchangeRate,
    IEnumerable<CreateSaleLineDto> Lines
);

public record UpdateSaleStatusDto(string Status);