namespace MedicalFurnitureAccounting.Models;

public class SupplyItem
{
    public int SupplyItemId { get; set; }
    public int SupplyId { get; set; }
    public virtual Supply Supply { get; set; }

    public int ProductId { get; set; }
    public virtual Product Product { get; set; }

    public int Count { get; set; }
}