namespace MedicalFurnitureAccounting.Models;

public class Material
{
    public Material(string name, int price)
    {
        Name = name;
        Price = price;
    }

    public Material()
    {
    }

    public int MaterialId { get; set; }

    public string Name { get; set; }

    public int Price { get; set; }

    public virtual ICollection<Product> Products { get; set; }
}