using Microsoft.EntityFrameworkCore;
using CPI.Domain.Purchase;
using SupplierEntity = CPI.Domain.Supplier.Supplier;
using TransactionEntity = CPI.Domain.Transaction.Transaction;



namespace CPI.Infrastructure
{
    public class CpiDbContext : DbContext
    {
        public CpiDbContext(DbContextOptions<CpiDbContext> options) : base(options) { }

        // --- PURCHASE ---
        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails => Set<PurchaseOrderDetail>();
        public DbSet<LogicalCost> LogicalCosts => Set<LogicalCost>();

        // --- RECEIPTS ---
        public DbSet<PurchaseReceipt> PurchaseReceipts => Set<PurchaseReceipt>();
        public DbSet<PurchaseReceiptDetail> PurchaseReceiptDetails => Set<PurchaseReceiptDetail>();

        // --- SUPPLIERS ---
        public DbSet<SupplierEntity> Suppliers => Set<SupplierEntity>();

        // --- TRANSACTIONS ---
        public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");


            //  PURCHASE MODULE

            modelBuilder.Entity<PurchaseOrder>(e =>
            {
                e.ToTable("PurchaseOrders");
                e.HasKey(x => x.PurchaseOrderId);

                e.Property(x => x.SupplierId).IsRequired();
                e.Property(x => x.OrderDate).IsRequired();
                e.Property(x => x.Status)
                    .HasMaxLength(20)
                    .IsRequired();

                e.HasMany(p => p.Details)
                 .WithOne(d => d.PurchaseOrder)
                 .HasForeignKey(d => d.PurchaseOrderId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PurchaseOrderDetail>(e =>
            {
                e.ToTable("PurchaseOrderDetails");
                e.HasKey(x => x.PurchaseOrderDetailId);

                e.Property(x => x.ProductId)
                 .IsRequired()
                 .HasMaxLength(50);

                e.Property(x => x.Quantity)
                 .HasPrecision(18, 4)
                 .IsRequired();

                e.Property(x => x.UnitPrice)
                 .HasPrecision(19, 4)
                 .IsRequired();

                e.Property(x => x.LineTotal)
                 .HasPrecision(38, 7)
                 .HasComputedColumnSql("[Quantity] * [UnitPrice]", stored: true);
            });

            modelBuilder.Entity<LogicalCost>(e =>
            {
                e.ToTable("LogicalCosts");
                e.HasKey(x => x.OrderNumber);

                e.Property(x => x.InternationalTransport).HasPrecision(19, 4);
                e.Property(x => x.LocalTransport).HasPrecision(19, 4);
                e.Property(x => x.Nationalization).HasPrecision(19, 4);
                e.Property(x => x.CargoInsurance).HasPrecision(19, 4);
                e.Property(x => x.Storage).HasPrecision(19, 4);
                e.Property(x => x.Others).HasPrecision(19, 4);

                e.HasOne(x => x.PurchaseOrder)
                    .WithOne(p => p.LogicalCost)
                    .HasForeignKey<LogicalCost>(x => x.OrderNumber);
            });


            //  RECEIPTS MODULE

            modelBuilder.Entity<PurchaseReceipt>(e =>
            {
                e.ToTable("PurchaseReceipts");
                e.HasKey(x => x.ReceiptId);
                e.Property(x => x.ReceiptDate).IsRequired();
                e.Property(x => x.PurchaseOrderId).IsRequired();
            });

            modelBuilder.Entity<PurchaseReceiptDetail>(e =>
            {
                e.ToTable("PurchaseReceiptDetails", tableBuilder =>
                {
                    // Desactiva el OUTPUT clause (si lo usas en SQL Server 2022)
                    tableBuilder.UseSqlOutputClause(false);
                });

                e.HasKey(x => x.ReceiptDetailId);

                e.Property(x => x.ProductId)
                  .HasMaxLength(50)
                  .IsRequired();

                e.Property(x => x.QuantityReceived)
                  .HasPrecision(18, 4)
                  .IsRequired();

                e.Property(x => x.UnitCost)
                  .HasPrecision(19, 4)
                  .IsRequired();

                e.HasOne<PurchaseReceipt>()
                  .WithMany(r => r.Details)
                  .HasForeignKey(d => d.ReceiptId)
                  .OnDelete(DeleteBehavior.Cascade);
            });


            // SUPPLIER MODULE

            modelBuilder.Entity<SupplierEntity>(e =>
            {
                e.ToTable("Suppliers");
                e.HasKey(x => x.SupplierId);

                e.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                e.Property(x => x.Contact).HasMaxLength(100);
                e.Property(x => x.Phone).HasMaxLength(20);
                e.Property(x => x.Email).HasMaxLength(100);
                e.Property(x => x.Address).HasMaxLength(255);

                e.HasIndex(x => x.Name);
                e.HasIndex(x => x.Email);
            });


            //  TRANSACTION MODULE

            modelBuilder.Entity<TransactionEntity>(e =>
            {
                e.ToTable("Transactions");
                e.HasKey(x => x.TransactionNumber);

                e.Property(x => x.InvoiceNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                e.Property(x => x.Reminder)
                    .HasMaxLength(100);

                e.Property(x => x.TransactionStatus)
                    .IsRequired()
                    .HasMaxLength(20);

                e.Property(x => x.PaymentDate)
                    .HasColumnType("date");

                e.Property(x => x.PurchaseOrderId).IsRequired();
            });
        }
    }
}
