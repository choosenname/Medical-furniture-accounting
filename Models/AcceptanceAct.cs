namespace MedicalFurnitureAccounting.Models;

public class AcceptanceAct
{
    public int AcceptanceActId { get; set; }
    public DateTime Date { get; set; }
    public string ProductName { get; set; }
    public string Category { get; set; }
    public int Count { get; set; }
    public string Room { get; set; }
    public string SupplierName { get; set; }
    public int SupplierId { get; set; }
}