using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InventoryService.Infrastructure.Data;

public class CpiDbContext : DbContext
{
    public CpiDbContext(DbContextOptions<CpiDbContext> options) : base(options) {}

    // DbSets
    public DbSet<User> Users => Set<User>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderDetail> PurchaseOrderDetails => Set<PurchaseOrderDetail>();
    public DbSet<LogicalCost> LogicalCosts => Set<LogicalCost>();
    public DbSet<PaymentTransaction> Transactions => Set<PaymentTransaction>(); // renombrado para evitar choque
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleDetail> SalesDetails => Set<SaleDetail>();
    public DbSet<PurchaseReceipt> PurchaseReceipts => Set<PurchaseReceipt>();
    public DbSet<PurchaseReceiptDetail> PurchaseReceiptDetails => Set<PurchaseReceiptDetail>();
    public DbSet<InventoryMovement> InventoryMovements => Set<InventoryMovement>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasDefaultSchema("dbo");

        b.Entity<User>().ToTable("Users").HasKey(x => x.Id);

        b.Entity<Supplier>().ToTable("Suppliers").HasKey(x => x.SupplierId);

        b.Entity<Client>().ToTable("Clients").HasKey(x => x.ClientId);
        b.Entity<Client>().HasIndex(x => new { x.DocumentType, x.DocumentID }).IsUnique();

        b.Entity<Product>().ToTable("Products").HasKey(x => x.ProductId);
        b.Entity<Product>()
            .HasOne<Supplier>()
            .WithMany()
            .HasForeignKey(x => x.SupplierId);

        b.Entity<PurchaseOrder>().ToTable("PurchaseOrders").HasKey(x => x.PurchaseOrderId);
        b.Entity<PurchaseOrder>()
            .HasOne<Supplier>()
            .WithMany()
            .HasForeignKey(x => x.SupplierId);

        b.Entity<PurchaseOrderDetail>().ToTable("PurchaseOrderDetails").HasKey(x => x.PurchaseOrderDetailId);
        b.Entity<PurchaseOrderDetail>()
            .HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId);
        b.Entity<PurchaseOrderDetail>()
            .HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);

        b.Entity<LogicalCost>().ToTable("LogicalCosts").HasKey(x => x.Order_Number);

        b.Entity<PaymentTransaction>().ToTable("Transactions").HasKey(x => x.Transaction_Number);
        b.Entity<PaymentTransaction>()
            .HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId);

        b.Entity<Sale>().ToTable("Sales").HasKey(x => x.InvoiceId);
        b.Entity<Sale>()
            .HasOne<Client>().WithMany().HasForeignKey(x => x.ClientId);

        b.Entity<SaleDetail>().ToTable("SalesDetails").HasKey(x => x.InvoiceDetailId);
        b.Entity<SaleDetail>()
            .HasOne<Sale>().WithMany().HasForeignKey(x => x.InvoiceId);
        b.Entity<SaleDetail>()
            .HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);

        b.Entity<PurchaseReceipt>().ToTable("PurchaseReceipts").HasKey(x => x.ReceiptId);
        b.Entity<PurchaseReceipt>()
            .HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId);

        b.Entity<PurchaseReceiptDetail>().ToTable("PurchaseReceiptDetails").HasKey(x => x.ReceiptDetailId);
        b.Entity<PurchaseReceiptDetail>()
            .HasOne<PurchaseReceipt>().WithMany().HasForeignKey(x => x.ReceiptId);
        b.Entity<PurchaseReceiptDetail>()
            .HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);

        b.Entity<InventoryMovement>().ToTable("InventoryMovements").HasKey(x => x.MovementId);
        b.Entity<InventoryMovement>()
            .HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
    }
}

// ====== POCOs ======
public class User
{
    [Key] public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
}

public class Supplier
{
    [Key] public int SupplierId { get; set; }
    public string Name { get; set; } = "";
    public string? Contact { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}

public class Client
{
    [Key] public int ClientId { get; set; }
    public string Name { get; set; } = "";
    public string ClientType { get; set; } = "";
    public string DocumentType { get; set; } = "";
    public string DocumentID { get; set; } = "";
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? Address { get; set; }
}

public class Product
{
    [Key] public string ProductId { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Value { get; set; }
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Stock { get; set; }
    public int SupplierId { get; set; }
}

public class PurchaseOrder
{
    [Key] public int PurchaseOrderId { get; set; }
    public int SupplierId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "";
}

public class PurchaseOrderDetail
{
    [Key] public int PurchaseOrderDetailId { get; set; }
    public int PurchaseOrderId { get; set; }
    public string ProductId { get; set; } = "";
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

public class LogicalCost
{
    [Key] public int Order_Number { get; set; }
    public decimal International_Transport { get; set; }
    public decimal Local_Transport { get; set; }
    public decimal Nationalization { get; set; }
    public decimal Cargo_Insurance { get; set; }
    public decimal Storage { get; set; }
    public decimal Others { get; set; }
}

public class PaymentTransaction
{
    [Key] public int Transaction_Number { get; set; }
    public int PurchaseOrderId { get; set; }
    public string Invoice_Number { get; set; } = "";
    public string? Reminder { get; set; }
    public string Transaction_Status { get; set; } = "";
    public DateTime? Payment_Date { get; set; }
}

public class Sale
{
    [Key] public int InvoiceId { get; set; }
    public int ClientId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string Status { get; set; } = "";
    public decimal? ExchangeRate { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
}

public class SaleDetail
{
    [Key] public int InvoiceDetailId { get; set; }
    public int InvoiceId { get; set; }
    public string ProductId { get; set; } = "";
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

public class PurchaseReceipt
{
    [Key] public int ReceiptId { get; set; }
    public int PurchaseOrderId { get; set; }
    public DateTime ReceiptDate { get; set; }
}

public class PurchaseReceiptDetail
{
    [Key] public int ReceiptDetailId { get; set; }
    public int ReceiptId { get; set; }
    public string ProductId { get; set; } = "";
    public decimal QuantityReceived { get; set; }
    public decimal UnitCost { get; set; }
}

public class InventoryMovement
{
    [Key] public int MovementId { get; set; }
    public string ProductId { get; set; } = "";
    public decimal QtyChange { get; set; }
    public string SourceType { get; set; } = "";
    public int? SourceId { get; set; }
    public DateTime CreatedAt { get; set; }
}
