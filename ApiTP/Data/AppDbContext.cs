using Microsoft.EntityFrameworkCore;
using AuthApi.Entities;

namespace AuthApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Mail)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
