using Microsoft.EntityFrameworkCore;
using SalesService.Domain.Entities;

namespace SalesService.Infrastructure.Data;

public class SalesDbContext : DbContext
{
    public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }

    public DbSet<Sale>       Sales       => Set<Sale>();
    public DbSet<SaleDetail> SaleDetails => Set<SaleDetail>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // ── Sales ────────────────────────────────────────────────────────────
        mb.Entity<Sale>(e =>
        {
            e.ToTable("Sales", tb => tb.UseSqlOutputClause(false));
            e.HasKey(s => s.InvoiceId);

            e.Property(s => s.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Emitida");

            e.Property(s => s.ExchangeRate).HasColumnType("decimal(18,4)");
            e.Property(s => s.Subtotal)    .HasColumnType("decimal(19,4)").HasDefaultValue(0m);
            e.Property(s => s.Tax)         .HasColumnType("decimal(19,4)").HasDefaultValue(0m);
            e.Property(s => s.Total)       .HasColumnType("decimal(19,4)").HasDefaultValue(0m);

            e.Property(s => s.InvoiceDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSUTCDATETIME()");

            e.HasMany(s => s.Details)
                .WithOne(d => d.Sale)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_SalesDetails_Sales")
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ── SaleDetails ──────────────────────────────────────────────────────
        mb.Entity<SaleDetail>(e =>
        {
            e.ToTable("SalesDetails", tb => tb.UseSqlOutputClause(false));
            e.HasKey(d => d.InvoiceDetailId);

            e.Property(d => d.ProductId) .HasMaxLength(50).IsRequired();
            e.Property(d => d.Quantity)  .HasColumnType("decimal(18,4)");
            e.Property(d => d.UnitPrice) .HasColumnType("decimal(19,4)");

            // LineTotal es columna calculada PERSISTED en BD → solo lectura en EF
            e.Property(d => d.LineTotal)
                .HasColumnType("decimal(19,4)")
                .ValueGeneratedOnAddOrUpdate();
        });
    }
}