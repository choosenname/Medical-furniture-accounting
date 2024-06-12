namespace MedicalFurnitureAccounting.Models;

public class Supply
{
    public Supply(DateTime date, int count, Supplier supplier, Product product)
    {
        Date = date;
        Count = count;
        Supplier = supplier;
    }

    public Supply()
    {
    }

    public int SupplyId { get; set; }

    public DateTime Date { get; set; }

    public int Count { get; set; }

    public int SupplierId { get; set; }
    public virtual Supplier Supplier { get; set; }

    public virtual ICollection<SupplyItem> SupplyItems { get; set; }

    public string ProductsString => string.Join(", ", SupplyItems.Select(s => s.Product.Name));
    public string CountString => string.Join(", ", SupplyItems.Select(s => s.Count));
}