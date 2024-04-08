namespace MedicalFurnitureAccounting.Models;

public class Supply
{
    public int SupplyId { get; set; }
    public DateTime Date { get; set; }
    // Другие свойства поставки

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }

    public ICollection<Product> Products { get; set; }
}