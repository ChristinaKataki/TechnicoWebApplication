﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechnicoWebApplication.Context;

#nullable disable

namespace TechnicoWebApplication.Migrations
{
    [DbContext(typeof(TechnicoDbContext))]
    [Migration("20250108211156_M5")]
    partial class M5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TechnicoWebApplication.Models.PropertyItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConstructionYear")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PropertyOwnerVat")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PropertyType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PropertyOwnerVat");

                    b.ToTable("PropertyItems");
                });

            modelBuilder.Entity("TechnicoWebApplication.Models.PropertyOwner", b =>
                {
                    b.Property<string>("Vat")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Vat");

                    b.ToTable("PropertyOwners");
                });

            modelBuilder.Entity("TechnicoWebApplication.Models.Repair", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Cost")
                        .HasColumnType("real");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PropertyItemId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("RepairDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeOfRepair")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PropertyItemId");

                    b.ToTable("Repairs");
                });

            modelBuilder.Entity("TechnicoWebApplication.Models.PropertyItem", b =>
                {
                    b.HasOne("TechnicoWebApplication.Models.PropertyOwner", "PropertyOwner")
                        .WithMany("PropertyItems")
                        .HasForeignKey("PropertyOwnerVat")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PropertyOwner");
                });

            modelBuilder.Entity("TechnicoWebApplication.Models.Repair", b =>
                {
                    b.HasOne("TechnicoWebApplication.Models.PropertyItem", "PropertyItem")
                        .WithMany("Repairs")
                        .HasForeignKey("PropertyItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PropertyItem");
                });

            modelBuilder.Entity("TechnicoWebApplication.Models.PropertyItem", b =>
                {
                    b.Navigation("Repairs");
                });

            modelBuilder.Entity("TechnicoWebApplication.Models.PropertyOwner", b =>
                {
                    b.Navigation("PropertyItems");
                });
#pragma warning restore 612, 618
        }
    }
}
