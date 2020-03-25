﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vantage.Data;

namespace Vantage.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Vantage.Common.Models.Attempt", b =>
                {
                    b.Property<int>("AttemptID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CumulativeLessonTime")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCompleted")
                        .HasColumnType("datetime2");

                    b.Property<int>("DriverID")
                        .HasColumnType("int");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<int>("LessonID")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<int>("TimeToComplete")
                        .HasColumnType("int");

                    b.HasKey("AttemptID");

                    b.HasIndex("DriverID");

                    b.HasIndex("LessonID");

                    b.ToTable("Attempts");
                });

            modelBuilder.Entity("Vantage.Common.Models.Driver", b =>
                {
                    b.Property<int>("DriverID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GroupID")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DriverID");

                    b.HasIndex("GroupID");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("Vantage.Common.Models.Group", b =>
                {
                    b.Property<int>("GroupID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.HasKey("GroupID");

                    b.HasIndex("ProductID");

                    b.ToTable("Groups");

                    b.HasData(
                        new
                        {
                            GroupID = 1,
                            Name = "Class 1",
                            ProductID = 2
                        },
                        new
                        {
                            GroupID = 2,
                            Name = "Class 2",
                            ProductID = 2
                        },
                        new
                        {
                            GroupID = 3,
                            Name = "Class 3",
                            ProductID = 2
                        },
                        new
                        {
                            GroupID = 4,
                            Name = "Class 4",
                            ProductID = 2
                        },
                        new
                        {
                            GroupID = 5,
                            Name = "Class 5",
                            ProductID = 2
                        },
                        new
                        {
                            GroupID = 6,
                            Name = "Class 6",
                            ProductID = 2
                        },
                        new
                        {
                            GroupID = 7,
                            Name = "Class 7",
                            ProductID = 2
                        },
                        new
                        {
                            GroupID = 8,
                            Name = "Class 8",
                            ProductID = 2
                        },
                        new
                        {
                            GroupID = 9,
                            Name = "Class 9",
                            ProductID = 2
                        });
                });

            modelBuilder.Entity("Vantage.Common.Models.Infraction", b =>
                {
                    b.Property<int>("InfractionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttemptID")
                        .HasColumnType("int");

                    b.Property<int>("Deduction")
                        .HasColumnType("int");

                    b.Property<string>("Enum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InfractionID");

                    b.HasIndex("AttemptID");

                    b.ToTable("Infractions");
                });

            modelBuilder.Entity("Vantage.Common.Models.Lesson", b =>
                {
                    b.Property<int>("LessonID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PackID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PackType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LessonID");

                    b.ToTable("Lessons");

                    b.HasData(
                        new
                        {
                            LessonID = 1,
                            IsActive = true,
                            Name = "Lesson 1",
                            PackID = "1",
                            PackType = "ABC123"
                        },
                        new
                        {
                            LessonID = 2,
                            IsActive = true,
                            Name = "Lesson 2",
                            PackID = "1",
                            PackType = "ABC123"
                        },
                        new
                        {
                            LessonID = 3,
                            IsActive = true,
                            Name = "Lesson 3",
                            PackID = "1",
                            PackType = "ABC123"
                        },
                        new
                        {
                            LessonID = 4,
                            IsActive = true,
                            Name = "Lesson 4",
                            PackID = "1",
                            PackType = "ABC123"
                        },
                        new
                        {
                            LessonID = 5,
                            IsActive = true,
                            Name = "Lesson 5",
                            PackID = "1",
                            PackType = "ABC123"
                        },
                        new
                        {
                            LessonID = 6,
                            IsActive = true,
                            Name = "Lesson 6",
                            PackID = "1",
                            PackType = "ABC123"
                        },
                        new
                        {
                            LessonID = 7,
                            IsActive = true,
                            Name = "Lesson 7",
                            PackID = "1",
                            PackType = "ABC123"
                        },
                        new
                        {
                            LessonID = 8,
                            IsActive = true,
                            Name = "Lesson 8",
                            PackID = "1",
                            PackType = "ABC123"
                        },
                        new
                        {
                            LessonID = 9,
                            IsActive = true,
                            Name = "Lesson 9",
                            PackID = "1",
                            PackType = "ABC123"
                        },
                        new
                        {
                            LessonID = 10,
                            IsActive = true,
                            Name = "Lesson 10",
                            PackID = "1",
                            PackType = "ABC123"
                        });
                });

            modelBuilder.Entity("Vantage.Common.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductID");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductID = 1,
                            Name = "One Simple Decision",
                            Version = "1.0"
                        },
                        new
                        {
                            ProductID = 2,
                            Name = "Virtual Driving Essentials",
                            Version = "1.0"
                        },
                        new
                        {
                            ProductID = 3,
                            Name = "Advanced Driver Safety",
                            Version = "1.0"
                        },
                        new
                        {
                            ProductID = 4,
                            Name = "Advanced Driver Safety - Fleet Edition",
                            Version = "1.0"
                        },
                        new
                        {
                            ProductID = 5,
                            Name = "One Simple Decision",
                            Version = "1.0"
                        });
                });

            modelBuilder.Entity("Vantage.Common.Models.Role", b =>
                {
                    b.Property<int>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleID");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleID = 1,
                            Name = "Instructor"
                        },
                        new
                        {
                            RoleID = 2,
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("Vantage.Common.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserID = 1,
                            FirstName = "Admin",
                            LastName = "Admin",
                            Password = "3FBFEB0EE307127BBD4EF7DA33F7B57A9FF3C7357DA182C5BFCCC2A4F599C6F9",
                            UserName = "Admin"
                        },
                        new
                        {
                            UserID = 3,
                            FirstName = "Admin",
                            LastName = "Admin",
                            Password = "CA978112CA1BBDCAFAC231B39A23DC4DA786EFF8147C4E72B9807785AFEE48BB",
                            UserName = "a"
                        },
                        new
                        {
                            UserID = 2,
                            FirstName = "John",
                            LastName = "Smith",
                            Password = "3FBFEB0EE307127BBD4EF7DA33F7B57A9FF3C7357DA182C5BFCCC2A4F599C6F9",
                            UserName = "JSmith"
                        });
                });

            modelBuilder.Entity("Vantage.Common.Models.UserRole", b =>
                {
                    b.Property<int>("UserRoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("UserRoleID");

                    b.HasIndex("RoleID");

                    b.HasIndex("UserID");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            UserRoleID = 1,
                            RoleID = 1,
                            UserID = 1
                        },
                        new
                        {
                            UserRoleID = 2,
                            RoleID = 2,
                            UserID = 1
                        },
                        new
                        {
                            UserRoleID = 3,
                            RoleID = 2,
                            UserID = 2
                        },
                        new
                        {
                            UserRoleID = 4,
                            RoleID = 1,
                            UserID = 3
                        },
                        new
                        {
                            UserRoleID = 5,
                            RoleID = 2,
                            UserID = 3
                        });
                });

            modelBuilder.Entity("Vantage.Common.Models.Attempt", b =>
                {
                    b.HasOne("Vantage.Common.Models.Driver", "Driver")
                        .WithMany("Attempts")
                        .HasForeignKey("DriverID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vantage.Common.Models.Lesson", "Lesson")
                        .WithMany("Attempts")
                        .HasForeignKey("LessonID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Vantage.Common.Models.Driver", b =>
                {
                    b.HasOne("Vantage.Common.Models.Group", "Group")
                        .WithMany("Drivers")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Vantage.Common.Models.Group", b =>
                {
                    b.HasOne("Vantage.Common.Models.Product", "Product")
                        .WithMany("Groups")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Vantage.Common.Models.Infraction", b =>
                {
                    b.HasOne("Vantage.Common.Models.Attempt", "Attempt")
                        .WithMany("Infractions")
                        .HasForeignKey("AttemptID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Vantage.Common.Models.UserRole", b =>
                {
                    b.HasOne("Vantage.Common.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vantage.Common.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
