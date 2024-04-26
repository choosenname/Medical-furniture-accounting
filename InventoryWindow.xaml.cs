using System.IO;
using System.Windows;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
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

    private void LoadSuppliers()
    {
        Products = _dbContext.Products.ToList();
        DataContext = this;
    }

    private void ExportToWordButton_Click(object sender, RoutedEventArgs e)
    {
        var helper = new WordHalper("Inventory.doc");

        var sum = Products.Sum(p => p.Count * p.Material.Price);

        var items = new Dictionary<string, string>
            {
                {"<DATE>", DateTime.Now.ToString("dd-MM-yyyy")  },
                { "<NAME>", user.Name},
                { "<SUM>", sum.ToString()},
            };

        helper.AddTableAndReplaceData(items, Products);
    }
}