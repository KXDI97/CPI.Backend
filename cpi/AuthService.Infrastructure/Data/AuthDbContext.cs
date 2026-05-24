using Microsoft.EntityFrameworkCore;
using AuthService.Domain;

namespace AuthService.Infrastructure.Data;   // 👈 usa Data, no Db

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<RefreshToken>(b =>
        {
            b.ToTable("RefreshTokens");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).HasColumnName("Id");
            b.Property(x => x.UserId).HasColumnName("UserId");
            b.Property(x => x.Token).HasColumnName("Token").HasMaxLength(256).IsRequired();
            b.Property(x => x.ExpiresAt).HasColumnName("ExpiresAt").HasColumnType("timestamp with time zone");
            b.Property(x => x.IsRevoked).HasColumnName("IsRevoked");
            b.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("timestamp with time zone")
             .HasDefaultValueSql("NOW()").ValueGeneratedOnAdd();
            b.HasIndex(x => x.Token).IsUnique();
        });

        mb.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.HasKey(x => x.ID);
            b.Property(x => x.ID).HasColumnName("ID");
            b.Property(x => x.Username).HasColumnName("Username").HasMaxLength(50).IsRequired();
            b.Property(x => x.Email).HasColumnName("Email").HasMaxLength(100).IsRequired();
            b.Property(x => x.Role).HasColumnName("Role").HasMaxLength(20).IsRequired();
            b.Property(x => x.PasswordHash).HasColumnName("PasswordHash");
            b.Property(x => x.PasswordSalt).HasColumnName("PasswordSalt");
            b.Property(x => x.CreatedAt)
             .HasColumnName("CreatedAt")
             .HasColumnType("timestamp with time zone")
             .HasDefaultValueSql("NOW()")
             .ValueGeneratedOnAdd();
            b.HasIndex(x => x.Username).IsUnique();
            b.HasIndex(x => x.Email).IsUnique();
        });
    }
}
