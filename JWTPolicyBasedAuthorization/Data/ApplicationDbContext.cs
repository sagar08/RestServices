using JWTPolicyBasedAuthorization.Extensions;
using JWTPolicyBasedAuthorization.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTPolicyBasedAuthorization.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ConfigKey> ConfigKeys { get; set; }
        public DbSet<ConfigValue> ConfigValues { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();

            ConfigKeyValueRelation(modelBuilder);
            UserRolesRelation(modelBuilder);            
            ProductConfigRelation(modelBuilder);            
        }

        private void UserRolesRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRoles>()
            .HasOne<User>(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRoles>()
            .HasOne<Role>(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
        }

        private void ConfigKeyValueRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigValue>()
            .HasOne<ConfigKey>(cv => cv.ConfigKey)
            .WithMany(ck => ck.ConfigValues)
            .HasForeignKey(cv => cv.KeyId);
        }

        private void ProductConfigRelation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
            .HasOne<ConfigValue>(p => p.Brand)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
            .HasOne<ConfigValue>(p => p.Category)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        }

    }
}