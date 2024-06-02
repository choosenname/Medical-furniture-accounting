namespace MedicalFurnitureAccounting.Models;

public class Supply
{
    public Supply(DateTime date, int count, Supplier supplier)
    {
        Date = date;
        Count = count;
        Supplier = supplier;
    }

    public int SupplyId { get; set; }

    public DateTime Date { get; set; }

    public int Count { get; set; }

    public int SupplierId { get; set; }
    public virtual Supplier Supplier { get; set; }

    public virtual ICollection<Product> Products { get; set; }
}