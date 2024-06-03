namespace MedicalFurnitureAccounting.Models;

public class Cell
{
    public Cell(int number)
    {
        Number = number;
    }

    public Cell()
    {
    }

    public int CellId { get; set; }

    public int Number { get; set; }
}