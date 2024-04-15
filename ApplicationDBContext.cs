using MedicalFurnitureAccounting.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalFurnitureAccounting;

public class ApplicationDBContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Supply> Supplies { get; set; }
    public DbSet<Storekeeper> Storekeepers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=MedicalFurnitureAccounting.db");
    }
}