using Microsoft.EntityFrameworkCore;
using PurchaseOrderService.Domain.Entities;

namespace PurchaseOrderService.Infrastructure.Data
{
    public class CpiDbContext : DbContext
    {
        public CpiDbContext(DbContextOptions<CpiDbContext> options) : base(options) { }

        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails => Set<PurchaseOrderDetail>();
        public DbSet<LogicalCost> LogicalCosts => Set<LogicalCost>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            // Tabla PurchaseOrders
            modelBuilder.Entity<PurchaseOrder>(e =>
            {
                e.ToTable("PurchaseOrders");
                e.HasKey(x => x.PurchaseOrderId);

                e.Property(x => x.SupplierId).IsRequired();
                e.Property(x => x.OrderDate).IsRequired();
                e.Property(x => x.Status)
                 .HasMaxLength(20)
                 .IsRequired();

                // Relación con detalles
                e.HasMany(p => p.Details)
                 .WithOne(d => d.PurchaseOrder)
                 .HasForeignKey(d => d.PurchaseOrderId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Tabla PurchaseOrderDetails
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

                // 🚀 Columna calculada: NO se inserta ni actualiza desde EF
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
        }
    }
}
