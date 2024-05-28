namespace MedicalFurnitureAccounting.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }

    public int Count { get; set; }

    public string Description { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int Length { get; set; }

    public int Weight { get; set; }

    public int Price { get; set; }

    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }

    public int SupplyId { get; set; }
    public virtual ICollection<Supply> Suppply { get; set; }

    public int MaterialId { get; set; }
    public virtual Material Material { get; set; }

    public int ShelvingId { get; set; }
    public virtual Shelving Shelving { get; set; }
}