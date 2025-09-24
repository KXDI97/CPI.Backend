using Microsoft.EntityFrameworkCore;
using AuthService.Domain;

namespace AuthService.Infrastructure.Data;   // 👈 usa Data, no Db

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.HasDefaultSchema("dbo");
        mb.Entity<User>(b =>
        {
            b.ToTable("Users", "dbo");
            b.HasKey(x => x.ID);
            b.Property(x => x.Username).HasMaxLength(50).IsRequired();
            b.Property(x => x.Email).HasMaxLength(100).IsRequired();
            b.Property(x => x.Role).HasMaxLength(10).IsRequired();
            b.Property(x => x.PasswordHash).HasColumnType("varbinary(256)");
            b.Property(x => x.PasswordSalt).HasColumnType("varbinary(128)");
            b.HasIndex(x => x.Username).IsUnique();
            b.HasIndex(x => x.Email).IsUnique();
        });
    }
}
