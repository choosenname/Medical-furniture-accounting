namespace MedicalFurnitureAccounting.Models;

public class Material
{
    public int MaterialId { get; set; }
    public string Name { get; set; }
    // Другие свойства материала

    public ICollection<Product> Products { get; set; }
}