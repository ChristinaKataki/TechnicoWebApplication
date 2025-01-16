using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoWebApplication.Models;

namespace TechnicoWebApplication.Context;
public class TechnicoDbContext : DbContext
{
    public DbSet<PropertyOwner> PropertyOwners { get; set; }
    public DbSet<PropertyItem> PropertyItems { get; set; }
    public DbSet<Repair> Repairs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source = localhost; Initial Catalog = Technico_EF; " +
        "Integrated Security = true; Encrypt=false");

        //optionsBuilder.UseSqlServer("Data Source = 192.168.1.48; Initial Catalog = Technico_EF; " +
        //        "User = 'sa'; Password = '!Abc123456'; Encrypt=false");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropertyItem>()
            .Property(p => p.PropertyType)
            .HasConversion<string>();

        modelBuilder.Entity<PropertyOwner>()
            .Property(p => p.UserType)
            .HasConversion<string>();

        modelBuilder.Entity<Repair>()
         .Property(p => p.Status)
         .HasConversion<string>();

        modelBuilder.Entity<Repair>()
         .Property(p => p.TypeOfRepair)
         .HasConversion<string>();

        modelBuilder.Entity<PropertyItem>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<PropertyOwner>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<Repair>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<PropertyOwner>()
            .HasMany(owner => owner.PropertyItems)
            .WithOne(item => item.PropertyOwner)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PropertyItem>()
            .HasMany(item => item.Repairs)
            .WithOne(repair => repair.PropertyItem)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Repair>()
        .HasIndex(repair => repair.Status)
        .HasDatabaseName("IX_Repair_Status");

        modelBuilder.Entity<Repair>()
            .HasIndex(repair => repair.RepairDate)
            .HasDatabaseName("IX_Repair_Date");

        modelBuilder.Entity<PropertyOwner>()
            .HasIndex(owner => owner.Email)
            .HasDatabaseName("IX_PropertyOwner_Email");
    }
}

