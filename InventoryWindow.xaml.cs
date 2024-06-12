using System.Windows;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting;

public partial class InventoryWindow : Window
{
    private readonly ApplicationDBContext _dbContext;
    private readonly Storekeeper user;

    public InventoryWindow(ApplicationDBContext dbContext, Storekeeper user)
    {
        _dbContext = dbContext;
        this.user = user;
        InitializeComponent();
        LoadSuppliers();
    }

    public ICollection<Product> Products { get; private set; }
    public ICollection<Supply> Supplies { get; private set; }

    private void LoadSuppliers()
    {
        Products = _dbContext.Products.ToList();
        Supplies = _dbContext.Supplies.ToList();
        DataContext = this;
    }

    private void ExportToWordButton_Click(object sender, RoutedEventArgs e)
    {
        var helper = new WordHelper("Docs/Inventory.doc");

        var sum = Supplies.Sum(p => p.SupplyItems.Sum(s => s.Count));

        var items = new Dictionary<string, string>
        {
            { "<DATE>", DateTime.Now.ToString("dd-MM-yyyy") },
            { "<NAME>", user.Name },
            { "<SUM>", sum.ToString() }
        };

        helper.AddTableAndReplaceData(items, Products);
    }
}