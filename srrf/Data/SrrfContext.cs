using Microsoft.EntityFrameworkCore;
using srrf.Models;

namespace srrf.Data
{
    public class SrrfContext : DbContext
    {
        public SrrfContext(DbContextOptions<SrrfContext> options) : base (options)
        { }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between ServiceRequest and Category
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(s => s.Category)
                .WithMany() // If Category can have multiple ServiceRequests, use WithMany()
                .HasForeignKey(s => s.CategoryId);
        }
    }
}
