using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
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
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
            .HasMany(c => c.Infractions)
            .WithOne(e => e.Product);

            #region Users

            modelBuilder.Entity<Role>().HasData(new Role { RoleID = 1, Name = "Instructor" });
            modelBuilder.Entity<Role>().HasData(new Role { RoleID = 2, Name = "Admin" });

            modelBuilder.Entity<User>().HasData(new User
            {
                UserID = 1,
                UserName = "Admin",
                FirstName = "Admin",
                LastName = "Admin",
                Password = GenerateSHA256String("P@55word")
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                UserID = 2,
                UserName = "JSmith",
                FirstName = "John",
                LastName = "Smith",
                Password = GenerateSHA256String("P@55word")
            });

            modelBuilder.Entity<UserRole>().HasData(new UserRole { UserRoleID = 1, RoleID = 1, UserID = 1 });
            modelBuilder.Entity<UserRole>().HasData(new UserRole { UserRoleID = 2, RoleID = 2, UserID = 1 });
            modelBuilder.Entity<UserRole>().HasData(new UserRole { UserRoleID = 3, RoleID = 2, UserID = 2 });

            #endregion

            #region SeedData

            modelBuilder.Entity<Product>().HasData(new Product { ProductID = 1, Name = "FleetDriver" });

            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }

            return result.ToString();
        }
    }
}
