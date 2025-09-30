using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Data;

public class CpiDbContext : DbContext
{
    public CpiDbContext(DbContextOptions<CpiDbContext> options) : base(options) { }

    public DbSet<Client> Clients => Set<Client>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Client>(e =>
        {
            e.ToTable("Clients", "dbo");
            e.HasKey(x => x.ClientId);

            e.Property(x => x.Name).IsRequired().HasMaxLength(150);
            e.Property(x => x.ClientType).IsRequired().HasMaxLength(20);
            e.Property(x => x.DocumentType).IsRequired().HasMaxLength(20);
            e.Property(x => x.DocumentID).IsRequired().HasMaxLength(40);
            e.Property(x => x.Email).HasMaxLength(100);
            e.Property(x => x.Phone).HasMaxLength(30);
            e.Property(x => x.Website).HasMaxLength(150);
            e.Property(x => x.Address).HasMaxLength(255);

            // UQ (DocumentType + DocumentID) como en tu script SQL
            e.HasIndex(x => new { x.DocumentType, x.DocumentID }).IsUnique()
            .HasDatabaseName("UQ_Clients_Doc");
        });
    }
}
