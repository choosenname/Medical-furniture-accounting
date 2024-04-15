using System.Windows;
using System.Windows.Controls;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddProductModal : Window
{
    private readonly ApplicationDBContext _dbContext;

    public AddProductModal(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
        LoadSuppliers();
    }

    public Product Product { get; private set; }

    private void LoadSuppliers()
    {
        var supplies = _dbContext.Supplies.ToList();

        SupplyComboBox.ItemsSource = supplies;
        SupplyComboBox.DisplayMemberPath = "Date";

        var categories = _dbContext.Categories.ToList();

        CategoryComboBox.ItemsSource = categories;
        CategoryComboBox.DisplayMemberPath = "Name";

        var materials = _dbContext.Materials.ToList();

        CategoryComboBox.ItemsSource = materials;
        CategoryComboBox.DisplayMemberPath = "Name";
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProductNameTextBox == null) return;

        var selectedSupply = (Supply)SupplyComboBox.SelectedItem;
        var selectedCategory = (Category)CategoryComboBox.SelectedItem;
        var materialCategory = (Material)MaterialComboBox.SelectedItem;

        Product = new Product
        {
            Name = ProductNameTextBox.Text,
            Suppply = selectedSupply,
            Category = selectedCategory,
            Material = materialCategory,
        };

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}