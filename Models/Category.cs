namespace MedicalFurnitureAccounting.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    // Другие свойства категории

    public ICollection<Product> Products { get; set; }
}