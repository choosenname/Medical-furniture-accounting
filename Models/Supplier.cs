namespace MedicalFurnitureAccounting.Models;

public class Supplier
{
    public int SupplierId { get; set; }
    public string Name { get; set; }

    public virtual Supply Supply { get; set; }
}