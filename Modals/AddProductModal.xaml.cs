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
        // Получаем список всех поставщиков из базы данных
        var suppliers = _dbContext.Suppliers.ToList();

        // Заполняем ComboBox списком поставщиков
        SupplierComboBox.ItemsSource = suppliers;
        SupplierComboBox.DisplayMemberPath = "Name"; // Указываем, какое свойство использовать для отображения
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (ProductNameTextBox == null) return;

        var selectedSupplier = (Supplier)SupplierComboBox.SelectedItem;

        Product = new Product
        {
            Name = ProductNameTextBox.Text,
            Supplier = selectedSupplier
        };

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}