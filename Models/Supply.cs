namespace MedicalFurnitureAccounting.Models;

public class Supply
{
    public Supply(DateTime date, int count, Supplier supplier, Product product)
    {
        Date = date;
        Count = count;
        Supplier = supplier;
        Product = product;
    }

    public Supply()
    {
    }

    public int SupplyId { get; set; }

    public DateTime Date { get; set; }

    public int Count { get; set; }

    public int SupplierId { get; set; }
    public virtual Supplier Supplier { get; set; }

    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
}