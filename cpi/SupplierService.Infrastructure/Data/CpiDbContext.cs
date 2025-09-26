using Microsoft.EntityFrameworkCore;
using SupplierEntity = SupplierService.Domain.Entities.Supplier;

namespace SupplierService.Infrastructure.Data;

public class CpiDbContext : DbContext
{
    public CpiDbContext(DbContextOptions<CpiDbContext> options) : base(options) { }

 public DbSet<SupplierEntity> Suppliers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");

        // Suppliers
        modelBuilder.Entity<SupplierEntity>(e =>
        {
            e.ToTable("Suppliers");
            e.HasKey(x => x.SupplierId);

            e.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(x => x.Contact)
                .HasMaxLength(100);

            e.Property(x => x.Phone)
                .HasMaxLength(20);

            e.Property(x => x.Email)
                .HasMaxLength(100);

            e.Property(x => x.Address)
                .HasMaxLength(255);

            // Índices
            e.HasIndex(x => x.Name);
            e.HasIndex(x => x.Email);
        });
    }
}
