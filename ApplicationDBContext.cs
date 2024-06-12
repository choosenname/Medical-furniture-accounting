using MedicalFurnitureAccounting.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalFurnitureAccounting;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext()
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Supply> Supplies { get; set; }
    public DbSet<Storekeeper> Storekeepers { get; set; }
    public DbSet<Shelving> Shelving { get; set; }
    public DbSet<Cell> Cell { get; set; }
    public DbSet<StoreHistory> StoreHistories { get; set; }
    public DbSet<SupplyItem> SupplyItems { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=MedicalFurnitureAccounting.db");
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Storekeeper>().HasData(
            new Storekeeper { StorekeeperId = 1, Name = "Admin", Password = "Admin" });

        base.OnModelCreating(modelBuilder);
    }
}