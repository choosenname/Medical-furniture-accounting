namespace MedicalFurnitureAccounting.Models;

public class Shelving
{
    public Shelving(int maxWeight, Cell cell)
    {
        MaxWeight = maxWeight;
        Cell = cell;
    }

    public Shelving()
    {
    }

    public int ShelvingId { get; set; }

    public int MaxWeight { get; set; }

    public int CellId { get; set; }
    public virtual Cell Cell { get; set; }
}