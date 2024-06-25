using ClaimsCompanyApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimsCompanyApi.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Claim> Claims { get; set; } 
        public DbSet<ClaimType> ClaimTypes { get; set; }
        public DbSet<Company> Companies { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Claim>()
                .HasKey(c => c.UCR);

            modelBuilder.Entity<Company>()
                .HasMany(c => c.Claims)
                .WithOne(cl => cl.Company)
                .HasForeignKey(cl => cl.CompanyId);
        }
    }
}
