using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Data // cambia a InventoryService.Infrastructure.Data en el otro proyecto
{
    public class CpiDbContext : DbContext
    {
        public CpiDbContext(DbContextOptions<CpiDbContext> options) : base(options) {}

        // Tablas principales
        public DbSet<User> Users => Set<User>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Product> Products => Set<Product>();

        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails => Set<PurchaseOrderDetail>();

        public DbSet<LogicalCost> LogicalCosts => Set<LogicalCost>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<SaleDetail> SalesDetails => Set<SaleDetail>();

        public DbSet<PurchaseReceipt> PurchaseReceipts => Set<PurchaseReceipt>();
        public DbSet<PurchaseReceiptDetail> PurchaseReceiptDetails => Set<PurchaseReceiptDetail>();

        public DbSet<InventoryMovement> InventoryMovements => Set<InventoryMovement>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            // Schemas / nombres
            b.HasDefaultSchema("dbo");

            // Users
            b.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(x => x.Id);
                e.Property(x => x.Username).IsRequired().HasMaxLength(50);
                e.Property(x => x.Email).IsRequired().HasMaxLength(100);
                e.Property(x => x.Role).IsRequired().HasMaxLength(10);
                e.HasIndex(x => x.Username).IsUnique();
                e.HasIndex(x => x.Email).IsUnique();
            });

            // Suppliers
            b.Entity<Supplier>(e =>
            {
                e.ToTable("Suppliers");
                e.HasKey(x => x.SupplierId);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            });

            // Clients
            b.Entity<Client>(e =>
            {
                e.ToTable("Clients");
                e.HasKey(x => x.ClientId);
                e.Property(x => x.Name).IsRequired().HasMaxLength(150);
                e.Property(x => x.ClientType).IsRequired().HasMaxLength(20);
                e.Property(x => x.DocumentType).IsRequired().HasMaxLength(20);
                e.Property(x => x.DocumentID).IsRequired().HasMaxLength(40);
                e.HasIndex(x => new { x.DocumentType, x.DocumentID }).IsUnique();
            });

            // Products
            b.Entity<Product>(e =>
            {
                e.ToTable("Products");
                e.HasKey(x => x.ProductId);
                e.Property(x => x.ProductId).HasMaxLength(50);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.Property(x => x.Value).HasPrecision(19, 4);
                e.Property(x => x.Stock).HasPrecision(18, 4);
                e.Property(x => x.Category).IsRequired().HasMaxLength(50);
                e.Property(x => x.Description).IsRequired().HasMaxLength(255);
                e.HasOne<Supplier>().WithMany().HasForeignKey(x => x.SupplierId);
            });

            // PurchaseOrders
            b.Entity<PurchaseOrder>(e =>
            {
                e.ToTable("PurchaseOrders");
                e.HasKey(x => x.PurchaseOrderId);
                e.Property(x => x.Status).IsRequired().HasMaxLength(20);
                e.HasOne<Supplier>().WithMany().HasForeignKey(x => x.SupplierId);
            });

            b.Entity<PurchaseOrderDetail>(e =>
            {
                e.ToTable("PurchaseOrderDetails");
                e.HasKey(x => x.PurchaseOrderDetailId);
                e.Property(x => x.Quantity).HasPrecision(18, 4);
                e.Property(x => x.UnitPrice).HasPrecision(19, 4);
                e.Property(x => x.LineTotal).HasPrecision(19, 4);
                e.HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId);
                e.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
            });

            // LogicalCosts
            b.Entity<LogicalCost>(e =>
            {
                e.ToTable("LogicalCosts");
                e.HasKey(x => x.Order_Number);
                e.Property(x => x.International_Transport).HasPrecision(19, 4);
                e.Property(x => x.Local_Transport).HasPrecision(19, 4);
                e.Property(x => x.Nationalization).HasPrecision(19, 4);
                e.Property(x => x.Cargo_Insurance).HasPrecision(19, 4);
                e.Property(x => x.Storage).HasPrecision(19, 4);
                e.Property(x => x.Others).HasPrecision(19, 4);
            });

            // Transactions
            b.Entity<Transaction>(e =>
            {
                e.ToTable("Transactions");
                e.HasKey(x => x.Transaction_Number);
                e.Property(x => x.Transaction_Status).IsRequired().HasMaxLength(20);
                e.HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId);
            });

            // Sales
            b.Entity<Sale>(e =>
            {
                e.ToTable("Sales");
                e.HasKey(x => x.InvoiceId);
                e.Property(x => x.Subtotal).HasPrecision(19, 4);
                e.Property(x => x.Tax).HasPrecision(19, 4);
                e.Property(x => x.Total).HasPrecision(19, 4);
                e.Property(x => x.ExchangeRate).HasPrecision(18, 4);
                e.HasOne<Client>().WithMany().HasForeignKey(x => x.ClientId);
            });

            b.Entity<SaleDetail>(e =>
            {
                e.ToTable("SalesDetails");
                e.HasKey(x => x.InvoiceDetailId);
                e.Property(x => x.Quantity).HasPrecision(18, 4);
                e.Property(x => x.UnitPrice).HasPrecision(19, 4);
                e.Property(x => x.LineTotal).HasPrecision(19, 4);
                e.HasOne<Sale>().WithMany().HasForeignKey(x => x.InvoiceId);
                e.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
            });

            // Purchase Receipts
            b.Entity<PurchaseReceipt>(e =>
            {
                e.ToTable("PurchaseReceipts");
                e.HasKey(x => x.ReceiptId);
                e.HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId);
            });

            b.Entity<PurchaseReceiptDetail>(e =>
            {
                e.ToTable("PurchaseReceiptDetails");
                e.HasKey(x => x.ReceiptDetailId);
                e.Property(x => x.QuantityReceived).HasPrecision(18, 4);
                e.Property(x => x.UnitCost).HasPrecision(19, 4);
                e.HasOne<PurchaseReceipt>().WithMany().HasForeignKey(x => x.ReceiptId);
                e.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
            });

            // InventoryMovements
            b.Entity<InventoryMovement>(e =>
            {
                e.ToTable("InventoryMovements");
                e.HasKey(x => x.MovementId);
                e.Property(x => x.QtyChange).HasPrecision(18, 4);
                e.Property(x => x.SourceType).IsRequired().HasMaxLength(20);
                e.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
            });
        }
    }

    // ===== Entidades mínimas (POCOs) =====
    public class User { public int Id { get; set; } public string Username { get; set; } = ""; public string Email { get; set; } = ""; public string Role { get; set; } = ""; }
    public class Supplier { public int SupplierId { get; set; } public string Name { get; set; } = ""; public string? Contact { get; set; } public string? Phone { get; set; } public string? Email { get; set; } public string? Address { get; set; } }
    public class Client { public int ClientId { get; set; } public string Name { get; set; } = ""; public string ClientType { get; set; } = ""; public string DocumentType { get; set; } = ""; public string DocumentID { get; set; } = ""; public string? Email { get; set; } public string? Phone { get; set; } public string? Website { get; set; } public string? Address { get; set; } }
    public class Product { public string ProductId { get; set; } = ""; public string Name { get; set; } = ""; public decimal Value { get; set; } public string Category { get; set; } = ""; public string Description { get; set; } = ""; public decimal Stock { get; set; } public int SupplierId { get; set; } }
    public class PurchaseOrder { public int PurchaseOrderId { get; set; } public int SupplierId { get; set; } public DateTime OrderDate { get; set; } public string Status { get; set; } = ""; }
    public class PurchaseOrderDetail { public int PurchaseOrderDetailId { get; set; } public int PurchaseOrderId { get; set; } public string ProductId { get; set; } = ""; public decimal Quantity { get; set; } public decimal UnitPrice { get; set; } public decimal LineTotal { get; set; } }
    public class LogicalCost { public int Order_Number { get; set; } public decimal International_Transport { get; set; } public decimal Local_Transport { get; set; } public decimal Nationalization { get; set; } public decimal Cargo_Insurance { get; set; } public decimal Storage { get; set; } public decimal Others { get; set; } }
    public class Transaction { public int Transaction_Number { get; set; } public int PurchaseOrderId { get; set; } public string Invoice_Number { get; set; } = ""; public string? Reminder { get; set; } public string Transaction_Status { get; set; } = ""; public DateTime? Payment_Date { get; set; } }
    public class Sale { public int InvoiceId { get; set; } public int ClientId { get; set; } public DateTime InvoiceDate { get; set; } public string Status { get; set; } = ""; public decimal? ExchangeRate { get; set; } public decimal Subtotal { get; set; } public decimal Tax { get; set; } public decimal Total { get; set; } }
    public class SaleDetail { public int InvoiceDetailId { get; set; } public int InvoiceId { get; set; } public string ProductId { get; set; } = ""; public decimal Quantity { get; set; } public decimal UnitPrice { get; set; } public decimal LineTotal { get; set; } }
    public class PurchaseReceipt { public int ReceiptId { get; set; } public int PurchaseOrderId { get; set; } public DateTime ReceiptDate { get; set; } }
    public class PurchaseReceiptDetail { public int ReceiptDetailId { get; set; } public int ReceiptId { get; set; } public string ProductId { get; set; } = ""; public decimal QuantityReceived { get; set; } public decimal UnitCost { get; set; } }
    public class InventoryMovement { public int MovementId { get; set; } public string ProductId { get; set; } = ""; public decimal QtyChange { get; set; } public string SourceType { get; set; } = ""; public int? SourceId { get; set; } public DateTime CreatedAt { get; set; } }
}
