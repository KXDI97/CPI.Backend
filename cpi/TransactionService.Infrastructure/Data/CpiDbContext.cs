using Microsoft.EntityFrameworkCore;
using TransactionEntity = TransactionService.Domain.Entities.Transaction;


namespace TransactionService.Infrastructure.Data;

public class CpiDbContext : DbContext
{
    public CpiDbContext(DbContextOptions<CpiDbContext> options) : base(options) { }

    public DbSet<TransactionEntity> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.HasDefaultSchema("dbo");

        b.Entity<TransactionEntity>(e =>
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

            // Relación con PurchaseOrder (si el microservicio Orders está separado, lo dejamos así)
            e.Property(x => x.PurchaseOrderId).IsRequired();
        });
    }
}
