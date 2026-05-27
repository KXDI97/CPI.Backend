using Microsoft.EntityFrameworkCore;
using PurchaseService.Domain.Entities;

namespace PurchaseService.Infrastructure.Data;

public class CpiDbContext : DbContext
{
    public CpiDbContext(DbContextOptions<CpiDbContext> options) : base(options) { }

    // ── Purchase ────────────────────────────────────────────────
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderDetail> PurchaseOrderDetails => Set<PurchaseOrderDetail>();
    public DbSet<PurchaseReceipt> PurchaseReceipts => Set<PurchaseReceipt>();
    public DbSet<PurchaseReceiptDetail> PurchaseReceiptDetails => Set<PurchaseReceiptDetail>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<LogicalCost> LogicalCosts => Set<LogicalCost>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // ── PurchaseOrder ────────────────────────────────────────
        mb.Entity<PurchaseOrder>(e =>
        {
            e.ToTable("PurchaseOrders");
            e.HasKey(x => x.PurchaseOrderId);
            e.Property(x => x.Status).HasMaxLength(20).IsRequired();
            e.Property(x => x.OrderDate).HasColumnType("datetime2");

            e.HasMany(x => x.Details)
             .WithOne(x => x.PurchaseOrder)
             .HasForeignKey(x => x.PurchaseOrderId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Receipt)
             .WithOne(x => x.PurchaseOrder)
             .HasForeignKey<PurchaseReceipt>(x => x.PurchaseOrderId);

            e.HasOne(x => x.Transaction)
             .WithOne(x => x.PurchaseOrder)
             .HasForeignKey<Transaction>(x => x.PurchaseOrderId);
        });

        // ── PurchaseOrderDetail ──────────────────────────────────
        mb.Entity<PurchaseOrderDetail>(e =>
        {
            e.ToTable("PurchaseOrderDetails");
            e.HasKey(x => x.PurchaseOrderDetailId);
            e.Property(x => x.ProductId).HasMaxLength(50).IsRequired();
            e.Property(x => x.Quantity).HasColumnType("decimal(18,4)");
            e.Property(x => x.UnitPrice).HasColumnType("decimal(19,4)");
            // LineTotal es columna calculada en BD, solo lectura
            e.Property(x => x.LineTotal).HasColumnType("decimal(19,4)")
             .ValueGeneratedOnAddOrUpdate();
        });

        // ── PurchaseReceipt ──────────────────────────────────────
        mb.Entity<PurchaseReceipt>(e =>
        {
            e.ToTable("PurchaseReceipts");
            e.HasKey(x => x.ReceiptId);
            e.Property(x => x.ReceiptDate).HasColumnType("datetime2");

            e.HasMany(x => x.Details)
             .WithOne(x => x.Receipt)
             .HasForeignKey(x => x.ReceiptId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── PurchaseReceiptDetail ────────────────────────────────
        mb.Entity<PurchaseReceiptDetail>(e =>
        {
            e.ToTable("PurchaseReceiptDetails");
            e.HasKey(x => x.ReceiptDetailId);
            e.Property(x => x.ProductId).HasMaxLength(50).IsRequired();
            e.Property(x => x.QuantityReceived).HasColumnType("decimal(18,4)");
            e.Property(x => x.UnitCost).HasColumnType("decimal(19,4)");
        });

        // ── Transaction ──────────────────────────────────────────
        mb.Entity<Transaction>(e =>
        {
            e.ToTable("Transactions");
            e.HasKey(x => x.TransactionNumber);
            // Nombres de columna exactos de la BD
            e.Property(x => x.TransactionNumber).HasColumnName("Transaction_Number");
            e.Property(x => x.InvoiceNumber).HasColumnName("Invoice_Number").HasMaxLength(50);
            e.Property(x => x.TransactionStatus).HasColumnName("Transaction_Status").HasMaxLength(20);
            e.Property(x => x.PaymentDate).HasColumnName("Payment_Date");
            e.Property(x => x.Reminder).HasMaxLength(100);
        });

        // ── LogicalCost ──────────────────────────────────────────────────────────
        mb.Entity<LogicalCost>(e =>
        {
            e.ToTable("LogicalCosts");
            e.HasKey(x => x.OrderNumber);
            e.Property(x => x.OrderNumber).HasColumnName("Order_Number");
            e.Property(x => x.InternationalTransport)
             .HasColumnName("International_Transport").HasColumnType("numeric(19,4)");
            e.Property(x => x.LocalTransport)
             .HasColumnName("Local_Transport").HasColumnType("numeric(19,4)");
            e.Property(x => x.Nationalization).HasColumnType("numeric(19,4)");
            e.Property(x => x.CargoInsurance)
             .HasColumnName("Cargo_Insurance").HasColumnType("numeric(19,4)");
            e.Property(x => x.Storage).HasColumnType("numeric(19,4)");
            e.Property(x => x.Others).HasColumnType("numeric(19,4)");

            // ← Esto le dice a EF que LogicalCost es el dependiente
            e.HasOne(x => x.PurchaseOrder)
             .WithOne(x => x.LogicalCost)
             .HasForeignKey<LogicalCost>(x => x.OrderNumber);
        });
    }
}