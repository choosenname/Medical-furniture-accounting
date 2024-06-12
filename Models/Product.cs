namespace MedicalFurnitureAccounting.Models;

public class Product
{
    public Product(string name, string description, int width, int height, int length, int weight,
        int price, Material material, Shelving shelving, Category category)
    {
        Name = name;
        Description = description;
        Width = width;
        Height = height;
        Length = length;
        Weight = weight;
        Price = price;
        Material = material;
        Shelving = shelving;
        Category = category;
    }

    public Product()
    {
    }

    public int ProductId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int Length { get; set; }

    public int Weight { get; set; }

    public int Price { get; set; }

    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }

    public virtual ICollection<SupplyItem> SupplyItems { get; set; }

    public int MaterialId { get; set; }
    public virtual Material Material { get; set; }

    public int ShelvingId { get; set; }
    public virtual Shelving Shelving { get; set; }

    public virtual ICollection<StoreHistory> StoreHistory { get; set; }

    public int Count => SupplyItems.Sum(s => s.Count);
}