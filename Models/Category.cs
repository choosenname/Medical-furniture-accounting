namespace MedicalFurnitureAccounting.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }

    public int ExtraCharge { get; set; }

    public virtual ICollection<Product> Products { get; set; }
}