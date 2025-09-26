using Microsoft.EntityFrameworkCore;
using PurchaseOrderService.Domain.Entities;

namespace PurchaseOrderService.Infrastructure.Data;

public class CpiDbContext : DbContext
{
    public CpiDbContext(DbContextOptions<CpiDbContext> options) : base(options) { }

    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderDetail> PurchaseOrderDetails => Set<PurchaseOrderDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");

        modelBuilder.Entity<PurchaseOrder>(e =>
        {
            e.ToTable("PurchaseOrders");
            e.HasKey(x => x.PurchaseOrderId);
            e.Property(x => x.SupplierId).IsRequired();
            e.Property(x => x.OrderDate).IsRequired();
        });

        modelBuilder.Entity<PurchaseOrderDetail>(e =>
        {
            e.ToTable("PurchaseOrderDetails");
            e.HasKey(x => x.PurchaseOrderDetailId);

            e.Property(x => x.ProductId).IsRequired().HasMaxLength(50);
            e.Property(x => x.Quantity).HasPrecision(18, 4).IsRequired();
            e.Property(x => x.UnitPrice).HasPrecision(19, 4).IsRequired();

            e.HasOne<PurchaseOrder>()
             .WithMany(p => p.Details)
             .HasForeignKey(d => d.PurchaseOrderId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
