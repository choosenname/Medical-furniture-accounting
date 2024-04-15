using System.Windows;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting;

public partial class InventoryWindow : Window
{
    private readonly ApplicationDBContext _dbContext;

    public InventoryWindow(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
        LoadSuppliers();
    }

    public ICollection<Product> Products { get; private set; }

    private void LoadSuppliers()
    {
        this.Products = _dbContext.Products.ToList();
        DataContext = this;
    }
}