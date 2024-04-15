namespace MedicalFurnitureAccounting.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public int SuppplyId { get; set; }
    public Supply Suppply { get; set; }

    public int MaterialId { get; set; }
    public Material Material { get; set; }
}