using Microsoft.EntityFrameworkCore;
using MRLanches.Users.API.Models;

namespace MRLanches.Users.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da tabela Users
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.UserType).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                // Índice único para username
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Seed inicial para usuários do sistema
            SeedSystemUsers(modelBuilder);
        }

        private void SeedSystemUsers(ModelBuilder modelBuilder)
        {
            // Hash da senha "1234567" para ADM
            var admPasswordHash = BCrypt.Net.BCrypt.HashPassword("1234567");
            var mrPasswordHash = BCrypt.Net.BCrypt.HashPassword("1234567");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "ADM",
                    PasswordHash = admPasswordHash,
                    UserType = UserType.ADM,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new User
                {
                    Id = 2,
                    Username = "MR",
                    PasswordHash = mrPasswordHash,
                    UserType = UserType.MR,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            );
        }
    }
}
