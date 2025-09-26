namespace TransactionService.Application.Transaction
{
    // DTO principal (lectura)
public record TransactionDto(
    int TransactionNumber,
    int PurchaseOrderId,
    string InvoiceNumber,
    string? Reminder,
    string TransactionStatus,
    DateTime? PaymentDate
);

// DTO para crear
public record CreateTransactionDto(
    int PurchaseOrderId,
    string InvoiceNumber,
    string? Reminder,
    string TransactionStatus,
    DateTime? PaymentDate
);

// DTO para actualizar
public record UpdateTransactionDto(
    string? Reminder,
    string TransactionStatus,
    DateTime? PaymentDate
);

}

