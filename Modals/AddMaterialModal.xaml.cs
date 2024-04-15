using System.Windows;
using MedicalFurnitureAccounting.Models;

namespace MedicalFurnitureAccounting.Modals;

public partial class AddMaterialModal : Window
{
    private readonly ApplicationDBContext _dbContext;

    public AddMaterialModal(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
        InitializeComponent();
        LoadSuppliers();
    }

    public Material Material { get; private set; }

    private void LoadSuppliers()
    {
        // Получаем список всех поставщиков из базы данных
        var products = _dbContext.Products.ToList();

        // Заполняем ComboBox списком поставщиков
        ProductComboBox.ItemsSource = products;
        ProductComboBox.DisplayMemberPath = "Name"; // Указываем, какое свойство использовать для отображения
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var selected = (Product)ProductComboBox.SelectedItem;

        Material = new Material
        {
            Name = MaterialNameTextBox.Text,
            Product = selected
        };

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        // Закрываем окно
        DialogResult = false;
    }
}