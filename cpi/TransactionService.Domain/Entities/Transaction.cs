using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionService.Domain.Entities
{
    public class Transaction
{
    [Column("Transaction_Number")]
    public int TransactionNumber { get; set; }
    public int PurchaseOrderId { get; set; }

    [Column("Invoice_Number")]
    public required string InvoiceNumber { get; set; }
    public required string Reminder { get; set; }

    [Column("Transaction_Status")]
    public required string TransactionStatus { get; set; }
    public DateTime? PaymentDate { get; set; }
}
}
