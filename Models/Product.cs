namespace MedicalFurnitureAccounting.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }

    public int Count { get; set; }

    public string Room { get; set; }

    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }

    public int SupplyId { get; set; }
    public virtual Supply Suppply { get; set; }

    public int MaterialId { get; set; }
    public virtual Material Material { get; set; }
}