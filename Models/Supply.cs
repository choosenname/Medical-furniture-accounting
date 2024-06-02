namespace MedicalFurnitureAccounting.Models;

public class Supply
{
    public Supply(DateTime date, int count, Supplier supplier, Product products)
    {
        Date = date;
        Count = count;
        Supplier = supplier;
        Products = products;
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
    //TODO: fix
    public virtual Product Products { get; set; }
}