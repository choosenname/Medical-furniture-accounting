namespace MedicalFurnitureAccounting.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public Material Material  { get; set; }

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
}