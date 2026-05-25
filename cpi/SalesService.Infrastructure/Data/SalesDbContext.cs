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

        bool isSqlServer = Database.ProviderName?.Contains("SqlServer") == true;

        // ── Sales ────────────────────────────────────────────────────────────
        mb.Entity<Sale>(e =>
        {
            if (isSqlServer)
                e.ToTable("Sales", tb => tb.UseSqlOutputClause(false));
            else
                e.ToTable("Sales");

            e.HasKey(s => s.InvoiceId);

            e.Property(s => s.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Emitida");

            e.Property(s => s.ExchangeRate).HasColumnType("decimal(18,4)");
            e.Property(s => s.Subtotal)    .HasColumnType("decimal(19,4)").HasDefaultValue(0m);
            e.Property(s => s.Tax)         .HasColumnType("decimal(19,4)").HasDefaultValue(0m);
            e.Property(s => s.Total)       .HasColumnType("decimal(19,4)").HasDefaultValue(0m);

            if (isSqlServer)
                e.Property(s => s.InvoiceDate)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("SYSUTCDATETIME()");
            else
                e.Property(s => s.InvoiceDate)
                    .HasDefaultValueSql("NOW()");

            e.HasMany(s => s.Details)
                .WithOne(d => d.Sale)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_SalesDetails_Sales")
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ── SaleDetails ──────────────────────────────────────────────────────
        mb.Entity<SaleDetail>(e =>
        {
            if (isSqlServer)
                e.ToTable("SalesDetails", tb => tb.UseSqlOutputClause(false));
            else
                e.ToTable("SalesDetails");

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