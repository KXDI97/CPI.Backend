using Microsoft.EntityFrameworkCore;
using AuthService.Domain;

namespace AuthService.Infrastructure.Data;   // 👈 usa Data, no Db

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<User>(b =>
        {
            b.ToTable("users");
            b.HasKey(x => x.ID);
            b.Property(x => x.ID).HasColumnName("id");
            b.Property(x => x.Username).HasColumnName("username").HasMaxLength(50).IsRequired();
            b.Property(x => x.Email).HasColumnName("email").HasMaxLength(100).IsRequired();
            b.Property(x => x.Role).HasColumnName("role").HasMaxLength(20).IsRequired();
            b.Property(x => x.PasswordHash).HasColumnName("password_hash");
            b.Property(x => x.PasswordSalt).HasColumnName("password_salt");
            b.Property(x => x.CreatedAt)
             .HasColumnName("created_at")
             .HasColumnType("timestamp with time zone")
             .HasDefaultValueSql("now()")
             .ValueGeneratedOnAdd();
            b.HasIndex(x => x.Username).IsUnique();
            b.HasIndex(x => x.Email).IsUnique();
        });
    }
}
