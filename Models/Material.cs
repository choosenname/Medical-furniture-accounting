namespace MedicalFurnitureAccounting.Models;

public class Material
{
    public int MaterialId { get; set; }
    public string Name { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }
}