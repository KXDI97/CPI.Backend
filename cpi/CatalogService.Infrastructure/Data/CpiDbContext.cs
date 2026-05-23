using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Data;

public class CpiDbContext : DbContext
{
    public CpiDbContext(DbContextOptions<CpiDbContext> options) : base(options) { }

    // ── DbSets ──────────────────────────────────────────────────────────────
    public DbSet<Client>   Clients   => Set<Client>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product>  Products  => Set<Product>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // ── Clients ─────────────────────────────────────────────────────────
        mb.Entity<Client>(e =>
        {
            e.ToTable("Clients");
            e.HasKey(c => c.ClientId);

            e.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            e.Property(c => c.ClientType)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Empresa");

            e.Property(c => c.DocumentType)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("RUT");

            e.Property(c => c.DocumentID)
                .IsRequired()
                .HasMaxLength(40);

            e.Property(c => c.Email)   .HasMaxLength(100);
            e.Property(c => c.Phone)   .HasMaxLength(30);
            e.Property(c => c.Website) .HasMaxLength(150);
            e.Property(c => c.Address) .HasMaxLength(255);

            // UNIQUE (DocumentType, DocumentID) → igual que BD
            e.HasIndex(c => new { c.DocumentType, c.DocumentID })
                .IsUnique()
                .HasDatabaseName("UQ_Clients_Doc");
        });

        // ── Suppliers ───────────────────────────────────────────────────────
        mb.Entity<Supplier>(e =>
        {
            e.ToTable("Suppliers");
            e.HasKey(s => s.SupplierId);

            e.Property(s => s.Name)   .IsRequired().HasMaxLength(100);
            e.Property(s => s.Contact).HasMaxLength(100);
            e.Property(s => s.Phone)  .HasMaxLength(20);
            e.Property(s => s.Email)  .HasMaxLength(100);
            e.Property(s => s.Address).HasMaxLength(255);
        });

        // ── Products ────────────────────────────────────────────────────────
        mb.Entity<Product>(e =>
        {
            e.ToTable("Products");
            e.HasKey(p => p.ProductId);

            e.Property(p => p.ProductId)  .HasMaxLength(50);
            e.Property(p => p.Name)       .IsRequired().HasMaxLength(100);
            e.Property(p => p.Value)      .HasColumnType("decimal(19,4)");
            e.Property(p => p.Category)   .IsRequired().HasMaxLength(50);
            e.Property(p => p.Description).IsRequired().HasMaxLength(255);
            e.Property(p => p.Stock)
                .HasColumnType("decimal(18,4)")
                .HasDefaultValue(0m);

            // FK → Suppliers
            e.HasOne<Supplier>()
                .WithMany()
                .HasForeignKey(p => p.SupplierId)
                .HasConstraintName("FK_Products_Suppliers")
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}