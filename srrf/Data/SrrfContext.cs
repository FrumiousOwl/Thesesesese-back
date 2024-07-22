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

            modelBuilder.Entity<ServiceRequest>()
                .HasOne(s => s.Category)
                .WithMany() 
                .HasForeignKey(s => s.CategoryId);


        }
    }
}
