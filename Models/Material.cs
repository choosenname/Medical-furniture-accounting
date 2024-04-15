namespace MedicalFurnitureAccounting.Models;

public class Material
{
    public int MaterialId { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public ICollection<Product> Products { get; set; }
}