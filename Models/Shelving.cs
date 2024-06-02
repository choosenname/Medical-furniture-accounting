namespace MedicalFurnitureAccounting.Models;

public class Shelving
{
    public int ShelvingId { get; set; }
    public int MaxWeight { get; set; }

    public int CellId { get; set; }
    public virtual Cell Cell { get; set; }
}