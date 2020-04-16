using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Vantage.Common.Models;

namespace Vantage.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public virtual DbSet<Infraction> Infractions { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<Attempt> Attempts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(c => c.Groups)
                .WithOne(e => e.Product)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(c => c.Lessons)
                .WithOne(e => e.Product)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Group>()
                .HasMany(c => c.Drivers)
                .WithOne(e => e.Group)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Driver>()
                .HasMany(c => c.Attempts)
                .WithOne(e => e.Driver)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attempt>()
                .HasMany(c => c.Infractions)
                .WithOne(e => e.Attempt)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lesson>()
                .HasMany(c => c.Attempts)
                .WithOne(e => e.Lesson)
                .OnDelete(DeleteBehavior.Restrict);

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
                UserID = 3,
                UserName = "a",
                FirstName = "Admin",
                LastName = "Admin",
                Password = GenerateSHA256String("a")
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
            modelBuilder.Entity<UserRole>().HasData(new UserRole { UserRoleID = 4, RoleID = 1, UserID = 3 });
            modelBuilder.Entity<UserRole>().HasData(new UserRole { UserRoleID = 5, RoleID = 2, UserID = 3 });

            #endregion

            #region SeedData

            modelBuilder.Entity<Product>().HasData(new Product { ProductID = 1, Name = "One Simple Decision", Version = "1.0" });
            modelBuilder.Entity<Product>().HasData(new Product { ProductID = 2, Name = "Virtual Driving Essentials", Version = "1.0" });
            modelBuilder.Entity<Product>().HasData(new Product { ProductID = 3, Name = "Advanced Driver Safety", Version = "1.0" });
            modelBuilder.Entity<Product>().HasData(new Product { ProductID = 4, Name = "Advanced Driver Safety - Fleet Edition", Version = "1.0" });
            modelBuilder.Entity<Product>().HasData(new Product { ProductID = 5, Name = "One Simple Decision", Version = "1.0" });

            modelBuilder.Entity<Group>().HasData(new Group { GroupID = 1, ProductID = 2, Name = "Class 1" });
            modelBuilder.Entity<Group>().HasData(new Group { GroupID = 2, ProductID = 2, Name = "Class 2" });
            modelBuilder.Entity<Group>().HasData(new Group { GroupID = 3, ProductID = 2, Name = "Class 3" });
            modelBuilder.Entity<Group>().HasData(new Group { GroupID = 4, ProductID = 2, Name = "Class 4" });
            modelBuilder.Entity<Group>().HasData(new Group { GroupID = 5, ProductID = 2, Name = "Class 5" });
            modelBuilder.Entity<Group>().HasData(new Group { GroupID = 6, ProductID = 2, Name = "Class 6" });
            modelBuilder.Entity<Group>().HasData(new Group { GroupID = 7, ProductID = 2, Name = "Class 7" });
            modelBuilder.Entity<Group>().HasData(new Group { GroupID = 8, ProductID = 2, Name = "Class 8" });
            modelBuilder.Entity<Group>().HasData(new Group { GroupID = 9, ProductID = 2, Name = "Class 9" });

            modelBuilder.Entity<Lesson>().HasData(new Lesson { LessonID = 1, PackType = "ABC123", PackID = "1", Name = "Lesson 1", IsActive = true, ProductID = 2, LessonOrder = 1 });
            modelBuilder.Entity<Lesson>().HasData(new Lesson { LessonID = 2, PackType = "ABC123", PackID = "1", Name = "Lesson 2", IsActive = true, ProductID = 2, LessonOrder = 2 });
            modelBuilder.Entity<Lesson>().HasData(new Lesson { LessonID = 3, PackType = "ABC123", PackID = "1", Name = "Lesson 3", IsActive = true, ProductID = 2, LessonOrder = 3 });
            modelBuilder.Entity<Lesson>().HasData(new Lesson { LessonID = 4, PackType = "ABC123", PackID = "1", Name = "Lesson 4", IsActive = true, ProductID = 2, LessonOrder = 4 });
            modelBuilder.Entity<Lesson>().HasData(new Lesson { LessonID = 5, PackType = "ABC123", PackID = "1", Name = "Lesson 5", IsActive = true, ProductID = 2, LessonOrder = 5 });
            modelBuilder.Entity<Lesson>().HasData(new Lesson { LessonID = 6, PackType = "ABC123", PackID = "1", Name = "Lesson 6", IsActive = true, ProductID = 2, LessonOrder = 6 });
            modelBuilder.Entity<Lesson>().HasData(new Lesson { LessonID = 7, PackType = "ABC123", PackID = "1", Name = "Lesson 7", IsActive = true, ProductID = 2, LessonOrder = 7 });
            modelBuilder.Entity<Lesson>().HasData(new Lesson { LessonID = 8, PackType = "ABC123", PackID = "1", Name = "Lesson 8", IsActive = true, ProductID = 2, LessonOrder = 8 });
            modelBuilder.Entity<Lesson>().HasData(new Lesson { LessonID = 9, PackType = "ABC123", PackID = "1", Name = "Lesson 9", IsActive = true, ProductID = 2, LessonOrder = 9 });
            modelBuilder.Entity<Lesson>().HasData(new Lesson { LessonID = 10, PackType = "ABC123", PackID = "1", Name = "Lesson 10", IsActive = true, ProductID = 2, LessonOrder = 10 });

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
