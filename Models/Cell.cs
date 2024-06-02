namespace MedicalFurnitureAccounting.Models;

public class Cell
{
    public Cell(int number)
    {
        Number = number;
    }

    public int CellId { get; set; }

    public int Number { get; set; }
}