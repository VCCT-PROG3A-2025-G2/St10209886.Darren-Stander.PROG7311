// Start of file
using Microsoft.EntityFrameworkCore;
using TestApp.Models;

namespace TestApp.Data
{
    // Start of ApplicationContext class
    public class ApplicationContext : DbContext
    {
        // Constructor: initializes DbContext with provided options
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        // DbSet representing User entities in the database
        public DbSet<User> Users { get; set; }

        // DbSet representing Farmer entities in the database
        public DbSet<Farmer> Farmers { get; set; }

        // DbSet representing Product entities in the database
        public DbSet<Product> Products { get; set; }

        // Configure entity relationships and cascade behaviors
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-one: Farmer → User
            modelBuilder.Entity<Farmer>()
                .HasOne<User>()                                // Each Farmer has one User
                .WithOne()                                     // Each User has one Farmer
                .HasForeignKey<Farmer>(f => f.UserId)          // Foreign key is Farmer.UserId
                .OnDelete(DeleteBehavior.Cascade);             // Delete Farmer when User is deleted

            // One-to-one: User → Farmer
            modelBuilder.Entity<User>()
                .HasOne<Farmer>()                              // Each User has one Farmer
                .WithOne()                                     // Each Farmer has one User
                .HasForeignKey<User>(u => u.FarmerId)          // Foreign key is User.FarmerId
                .OnDelete(DeleteBehavior.Cascade);             // Delete User when Farmer is deleted

            // One-to-many: Farmer → Products
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Farmer)                         // Each Product has one Farmer
                .WithMany()                                    // A Farmer can have many Products
                .HasForeignKey(p => p.FarmerId)                // Foreign key is Product.FarmerId
                .OnDelete(DeleteBehavior.Cascade);             // Delete Products when Farmer is deleted
        }
    }
    // End of ApplicationContext class
}
// End of file
