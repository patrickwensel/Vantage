using Microsoft.EntityFrameworkCore;
using Vantage.Data.Models;

namespace Vantage.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public virtual DbSet<Infraction> Infractions { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
            .HasMany(c => c.Infractions)
            .WithOne(e => e.Product);

            #region Users

            #endregion

            #region SeedData

            modelBuilder.Entity<Product>().HasData(new Product { ProductID = 1, Name = "FleetDriver" });

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
