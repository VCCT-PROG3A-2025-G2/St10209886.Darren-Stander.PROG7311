using Microsoft.EntityFrameworkCore;
using TestApp.Models;

namespace TestApp.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Farmer>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Farmer>(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne<Farmer>()
                .WithOne()
                .HasForeignKey<User>(u => u.FarmerId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔥 Link Product to Farmer
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Farmer)
                .WithMany()
                .HasForeignKey(p => p.FarmerId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
