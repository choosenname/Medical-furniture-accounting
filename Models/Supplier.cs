namespace MedicalFurnitureAccounting.Models;

public class Supplier
{
    public int SupplierId { get; set; }
    public string Name { get; set; }
        
    public Supply Supply { get; set; }
}