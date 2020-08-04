using JwtAuthWithRefreshToken.Data.Entities;
using JwtAuthWithRefreshToken.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthWithRefreshToken.Data
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options) { 
            
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.RemovePluralizingTableNameConvention();
            
        }
    }
}