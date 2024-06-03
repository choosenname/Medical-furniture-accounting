namespace MedicalFurnitureAccounting.Models;

public class StoreHistory
{
    public StoreHistory(DateTime date, Product product, Shelving shelving)
    {
        Date = date;
        Product = product;
        Shelving = shelving;
    }

    public StoreHistory(Product product, Shelving shelving)
    {
        Date = DateTime.Now;
        Product = product;
        Shelving = shelving;
    }

    public StoreHistory()
    {
    }

    public int StoreHistoryId { get; set; }

    public DateTime Date { get; set; }

    public int ProductId { get; set; }
    public virtual Product Product { get; set; }

    public int ShelvingId { get; set; }
    public virtual Shelving Shelving { get; set; }
}