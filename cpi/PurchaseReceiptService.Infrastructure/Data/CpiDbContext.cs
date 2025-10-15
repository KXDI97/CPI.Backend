using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using PurchaseReceiptService.Application.Purchase;  
using PurchaseReceiptService.Domain.Entities;       
using PurchaseReceiptService.Infrastructure.Data;   

namespace PurchaseReceiptService.Infrastructure.Data;

public class CpiDbContext : DbContext
{
    public CpiDbContext(DbContextOptions<CpiDbContext> options) : base(options) { }

    public DbSet<PurchaseReceipt> PurchaseReceipts => Set<PurchaseReceipt>();
    public DbSet<PurchaseReceiptDetail> PurchaseReceiptDetails => Set<PurchaseReceiptDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");

        modelBuilder.Entity<PurchaseReceipt>(e =>
        {
            e.ToTable("PurchaseReceipts");
            e.HasKey(x => x.ReceiptId);
            e.Property(x => x.ReceiptDate).IsRequired();
            e.Property(x => x.PurchaseOrderId).IsRequired();
        });

modelBuilder.Entity<PurchaseReceiptDetail>(entity =>
{
    entity.ToTable("PurchaseReceiptDetails", tableBuilder =>
    {
        //Esto es para desactiva el OUTPUT clause para esta tabla
        tableBuilder.UseSqlOutputClause(false);
    });

    entity.HasKey(x => x.ReceiptDetailId);

    entity.Property(x => x.ProductId)
          .HasMaxLength(50)
          .IsRequired();

    entity.Property(x => x.QuantityReceived)
          .HasPrecision(18, 4)
          .IsRequired();

    entity.Property(x => x.UnitCost)
          .HasPrecision(19, 4)
          .IsRequired();

    entity.HasOne<PurchaseReceipt>()
          .WithMany(r => r.Details)
          .HasForeignKey(d => d.ReceiptId)
          .OnDelete(DeleteBehavior.Cascade);
});
    }
}
