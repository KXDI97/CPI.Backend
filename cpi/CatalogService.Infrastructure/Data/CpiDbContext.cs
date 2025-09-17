using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Data;

public class CpiDbContext : DbContext
{
    public CpiDbContext(DbContextOptions<CpiDbContext> options) : base(options) { }

    // DbSets principales
    // public DbSet<User> Users => Set<User>();
    // public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Client> Clients => Set<Client>();
    //public DbSet<Product> Products => Set<Product>();
    // public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    // public DbSet<PurchaseOrderDetail> PurchaseOrderDetails => Set<PurchaseOrderDetail>();
    // public DbSet<LogicalCost> LogicalCosts => Set<LogicalCost>();
    // public DbSet<Transaction> Transactions => Set<Transaction>();
    // public DbSet<Sale> Sales => Set<Sale>();
    // public DbSet<SaleDetail> SalesDetails => Set<SaleDetail>();
    // public DbSet<PurchaseReceipt> PurchaseReceipts => Set<PurchaseReceipt>();
    // public DbSet<PurchaseReceiptDetail> PurchaseReceiptDetails => Set<PurchaseReceiptDetail>();
    // public DbSet<InventoryMovement> InventoryMovements => Set<InventoryMovement>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasDefaultSchema("dbo");

        // Users
        //   b.Entity<User>(e =>
        //   {
        //       e.ToTable("Users");
        //       e.HasKey(x => x.Id);
        //       e.Property(x => x.Username).IsRequired().HasMaxLength(50);
        //      e.Property(x => x.Email).IsRequired().HasMaxLength(100);
        //      e.Property(x => x.Role).IsRequired().HasMaxLength(10);
        //      e.HasIndex(x => x.Username).IsUnique();
        //      e.HasIndex(x => x.Email).IsUnique();
        //   });

        // Suppliers
        //    b.Entity<Supplier>(e =>
       //{
        //    e.ToTable("Suppliers");
        //    e.HasKey(x => x.SupplierId);

        //    e.Property(x => x.Name)   .IsRequired().HasMaxLength(100);
        //    e.Property(x => x.Contact).HasMaxLength(100);
        //    e.Property(x => x.Phone)  .HasMaxLength(30);
        //    e.Property(x => x.Email)  .HasMaxLength(100);
        //    e.Property(x => x.Address).HasMaxLength(255);

        //    e.HasIndex(x => x.Name);
        //    e.HasIndex(x => x.Email);

            // Relaciones opcionales: descomenta cuando actives Product/PurchaseOrder
            // e.HasMany<Product>().WithOne().HasForeignKey(p => p.SupplierId).OnDelete(DeleteBehavior.Restrict);
            // e.HasMany<PurchaseOrder>().WithOne().HasForeignKey(po => po.SupplierId).OnDelete(DeleteBehavior.Restrict);
       // });

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
        //    b.Entity<Product>(e =>
        //    {
        //        e.ToTable("Products");
        //        e.HasKey(x => x.ProductId);
        //        e.Property(x => x.ProductId).HasMaxLength(50);
        //        e.Property(x => x.Name).IsRequired().HasMaxLength(100);
        //        e.Property(x => x.Value).HasPrecision(19, 4);
        //        e.Property(x => x.Stock).HasPrecision(18, 4);
        //       e.Property(x => x.Category).IsRequired().HasMaxLength(50);
        //        e.Property(x => x.Description).IsRequired().HasMaxLength(255);
        //        e.HasOne<Supplier>().WithMany().HasForeignKey(x => x.SupplierId);
        //    });

        // PurchaseOrders
        //    b.Entity<PurchaseOrder>(e =>
        //    {
        //        e.ToTable("PurchaseOrders");
        //        e.HasKey(x => x.PurchaseOrderId);
        //        e.Property(x => x.Status).IsRequired().HasMaxLength(20);
        //       e.HasOne<Supplier>().WithMany().HasForeignKey(x => x.SupplierId);
        //    });

        //    b.Entity<PurchaseOrderDetail>(e =>
        //    {
        //        e.ToTable("PurchaseOrderDetails");
        //        e.HasKey(x => x.PurchaseOrderDetailId);
        //       e.Property(x => x.Quantity).HasPrecision(18, 4);
        //        e.Property(x => x.UnitPrice).HasPrecision(19, 4);
        //        e.Property(x => x.LineTotal).HasPrecision(19, 4);
        //        e.HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId);
        //        e.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
        //    });

        // LogicalCosts
        //    b.Entity<LogicalCost>(e =>
        //    {
        //       e.ToTable("LogicalCosts");
        //        e.HasKey(x => x.Order_Number);
        //        e.Property(x => x.International_Transport).HasPrecision(19, 4);
        //        e.Property(x => x.Local_Transport).HasPrecision(19, 4);
        //        e.Property(x => x.Nationalization).HasPrecision(19, 4);
        //        e.Property(x => x.Cargo_Insurance).HasPrecision(19, 4);
        //        e.Property(x => x.Storage).HasPrecision(19, 4);
        //       e.Property(x => x.Others).HasPrecision(19, 4);
        //    });

        // Transactions
        //    b.Entity<Transaction>(e =>
        //    {
        //        e.ToTable("Transactions");
        //        e.HasKey(x => x.Transaction_Number);
        //       e.Property(x => x.Transaction_Status).IsRequired().HasMaxLength(20);
        //    });

        // Sales
        //    b.Entity<Sale>(e =>
        //    {
        //        e.ToTable("Sales");
        //       e.HasKey(x => x.InvoiceId);
        //       e.Property(x => x.Subtotal).HasPrecision(19, 4);
        //       e.Property(x => x.Tax).HasPrecision(19, 4);
        //       e.Property(x => x.Total).HasPrecision(19, 4);
        //       e.Property(x => x.ExchangeRate).HasPrecision(18, 4);
        //       e.HasOne<Client>().WithMany().HasForeignKey(x => x.ClientId);
        //   });

        //   b.Entity<SaleDetail>(e =>
        //   {
        //       e.ToTable("SalesDetails");
        //        e.HasKey(x => x.InvoiceDetailId);
        //        e.Property(x => x.Quantity).HasPrecision(18, 4);
        //        e.Property(x => x.UnitPrice).HasPrecision(19, 4);
        //        e.Property(x => x.LineTotal).HasPrecision(19, 4);
        //        e.HasOne<Sale>().WithMany().HasForeignKey(x => x.InvoiceId);
        //        e.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
        //    });

        // Purchase Receipts
        //    b.Entity<PurchaseReceipt>(e =>
        //   {
        //        e.ToTable("PurchaseReceipts");
        //        e.HasKey(x => x.ReceiptId);
        //        e.HasOne<PurchaseOrder>().WithMany().HasForeignKey(x => x.PurchaseOrderId);
        //   });

        //   b.Entity<PurchaseReceiptDetail>(e =>
        //   {
        //       e.ToTable("PurchaseReceiptDetails");
        //       e.HasKey(x => x.ReceiptDetailId);
        //       e.Property(x => x.QuantityReceived).HasPrecision(18, 4);
        //       e.Property(x => x.UnitCost).HasPrecision(19, 4);
        //       e.HasOne<PurchaseReceipt>().WithMany().HasForeignKey(x => x.ReceiptId);
        //       e.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
        //   });

        // InventoryMovements
        //   b.Entity<InventoryMovement>(e =>
        //   {
        //       e.ToTable("InventoryMovements");
        //       e.HasKey(x => x.MovementId);
        //       e.Property(x => x.QtyChange).HasPrecision(18, 4);
        //       e.Property(x => x.SourceType).IsRequired().HasMaxLength(20);
        //       e.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId);
        ////   });
        // }
    }
}
