﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShirlyStudio.Models;

namespace ShirlyStudio.Migrations
{
    [DbContext(typeof(ShirlyStudioContext))]
    [Migration("20181106185823_ai")]
    partial class ai
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ShirlyStudio.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .IsRequired();

                    b.HasKey("CategoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("ShirlyStudio.Models.ClusterResulter", b =>
                {
                    b.Property<int>("ClusterResulterID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClusterRes");

                    b.Property<int>("WokshopId");

                    b.HasKey("ClusterResulterID");

                    b.ToTable("ClusterResulter");
                });

            modelBuilder.Entity("ShirlyStudio.Models.CustomerRegistration", b =>
                {
                    b.Property<int>("CustomerRegistrationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CustomerId");

                    b.Property<int>("WorkshopId");

                    b.HasKey("CustomerRegistrationId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("WorkshopId");

                    b.ToTable("CustomerRegistration");
                });

            modelBuilder.Entity("WebApplication4.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age");

                    b.Property<string>("CustomerName")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.HasKey("CustomerId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("WebApplication4.Models.Teacher", b =>
                {
                    b.Property<int>("TeacherId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TeacherName")
                        .IsRequired();

                    b.HasKey("TeacherId");

                    b.ToTable("Teacher");
                });

            modelBuilder.Entity("WebApplication4.Models.Workshop", b =>
                {
                    b.Property<int>("WorkshopId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Available_Members");

                    b.Property<int>("CategoryId");

                    b.Property<string>("Description");

                    b.Property<double>("Duration");

                    b.Property<DateTime>("FullData");

                    b.Property<int>("Price");

                    b.Property<int>("TeacherId");

                    b.Property<string>("WorkshopName")
                        .IsRequired();

                    b.HasKey("WorkshopId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Workshop");
                });

            modelBuilder.Entity("ShirlyStudio.Models.CustomerRegistration", b =>
                {
                    b.HasOne("WebApplication4.Models.Customer", "Customer")
                        .WithMany("CustomerRegistration")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApplication4.Models.Workshop", "Workshop")
                        .WithMany("CustomerRegistrations")
                        .HasForeignKey("WorkshopId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApplication4.Models.Workshop", b =>
                {
                    b.HasOne("ShirlyStudio.Models.Category", "Category")
                        .WithMany("Workshops")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApplication4.Models.Teacher", "Teacher")
                        .WithMany("Workshops")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
